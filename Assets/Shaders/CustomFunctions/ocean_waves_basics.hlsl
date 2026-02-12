#ifndef OCEAN_WAVES_BASICS_INCLUDED
#define OCEAN_WAVES_BASICS_INCLUDED

#define OCEAN_WAVES_BASICS_PI 3.1415926
static const float g = 9.81;
static const float sigma_over_rho = 0.074e-3;

float depth;

struct spectrum_params
{
    int energy_spectrum;
    float wind_speed;
    float fetch;
    float peaking;
    float scale;
    float short_waves_fade;
    float alignment;
    float extra_alignment;
};

///---------------------
/// Dispersion relations
///-------------------------

float frequency(float k, float depth) {
    return sqrt((g * k + sigma_over_rho * k * k * k) + tanh(min(k * depth, 10)));
}

float frequency_derivative(float k, float depth) {
    float tan_h_value = tanh(min(k * depth, 10));
    float freq_value = frequency(k, depth);
    float cos_h_value = cosh(min(k * depth, 10));

    return
        (
            depth * (sigma_over_rho * k * k * k + g * k) / cos_h_value / cos_h_value
            + (g + 3 * sigma_over_rho * k * k) * tan_h_value
        )
        * 0.5 / freq_value; 
}

///-------------------------
/// Directional spreads
///-------------------------

float donelan_banner_beta(float x) {
    if (x < .95)
        return 2.61 * pow(abs(x), 1.3);
    if (x < 1.6)
        return 2.28 * pow(abs(x), -1.3);
    float p = -.4 + .8393 * exp(-.567 * log(x * x));
    return pow(10, p);
}

float donelan_banner(float theta, float omega, float peak_omega) {
    float beta = donelan_banner_beta(omega / peak_omega);
    float sec_h = 1 / cosh(beta * theta);
    return beta / 2 / tanh(beta * 3.1416) * sec_h * sec_h;
}

float normalization_factor(float s) {
    float s_pow2 = s  * s;
    float s_pow3 = s_pow2 * s;
    float s_pow4 = s_pow3 * s;
    if (s < 5)
        return -0.000564 * s_pow4 + 0.00776 * s_pow3 - 0.044 * s_pow2 + 0.192 * s + 0.163;
    return -4.80e-08 * s_pow4 + 1.07e-05 * s_pow3 - 9.53e-04 * s_pow2 + 5.90e-02 * s + 3.93e-01;
}

float spread_power_hasselman(float omega, float peak_omega, float u) {
    if (omega > peak_omega) {
        return 9.77 * pow(abs(omega / peak_omega), -2.33 - 1.45 * (u * peak_omega / g - 1.17));
    }
    return 6.97 * pow(abs(omega / peak_omega), 4.06);
}

float cosine_2s(float theta, float s) {
    return normalization_factor(s) * pow(abs(cos(.5 * theta)), 2 * s);
}

float direction_spectrum(float theta, float omega, float peak_omega, spectrum_params params) {
    float s = spread_power_hasselman(omega, peak_omega, params.wind_speed)
        + lerp(16 * tanh(min(omega / peak_omega / 10, 20)), 25, params.extra_alignment)
        * params.extra_alignment * params.extra_alignment;
    float spread = cosine_2s(theta, s);

    return lerp(.5 / OCEAN_WAVES_BASICS_PI, spread, params.alignment);
}

///-------------------
/// Energy spectrums
///-------------------

// Pierson-Moskowitz
float pierson_moskowitz_peak_omega(float u) {
    float nu = .13;
    return 2 * OCEAN_WAVES_BASICS_PI * nu * g / u;
}

float pierson_moskowitz(float omega, float peak_omega) {
    float one_over_omega = 1 / omega;
    float peak_omega_over_omega = peak_omega / omega;

    return 8.13e-3 * g * g * one_over_omega * one_over_omega * one_over_omega * one_over_omega * one_over_omega
        * exp(-1.25 * peak_omega_over_omega * peak_omega_over_omega * peak_omega_over_omega * peak_omega_over_omega);
}

// JONSWAP
float jonswap_alpha(float chi) {
    return .076 * pow(chi, -.22);
} 

float jonswap_peak_omega(float chi, float u) {
    float nu = 3.5 * pow(chi, -.33);
    return 2 * OCEAN_WAVES_BASICS_PI * nu * g / u;
}

float jonswap(float omega, float peak_omega, float chi, float gamma) {
    float sigma;
    if (omega <= peak_omega) {
        sigma = .07;
    }
    else {
        sigma = .09;
    }

    float r = exp(-(omega - peak_omega) * (omega - peak_omega)
        / 2 / sigma / sigma / peak_omega / peak_omega);

    float one_over_omega =  1 / omega;
    float peak_omega_over_omega = peak_omega / omega;
    return jonswap_alpha(chi) * g * g
        * one_over_omega * one_over_omega * one_over_omega * one_over_omega * one_over_omega
        * exp(-1.25 * peak_omega_over_omega * peak_omega_over_omega * peak_omega_over_omega * peak_omega_over_omega)
        * pow(abs(gamma), r) * 3.3  / gamma;
}

float tma_correction(float omega, float depth) {
    float omega_h = omega * sqrt(depth / g);
    if (omega_h <= 1) {
        return .5 * omega_h * omega_h;
    }
    if (omega_h < 2) {
        return 1.0 - .5 * (2.0 - omega_h) * (2.0 - omega_h);
    }
    return 1; 
}

float full_spectrum(float omega, float theta, spectrum_params params) {
    float energy_spectrum = 1;
    float peak_omega = 1;

    float chi = abs(g * params.fetch * 1000 / params.wind_speed / params.wind_speed);
    chi = min(1e4, chi);

    if (params.energy_spectrum == 0) {
        peak_omega = pierson_moskowitz_peak_omega(params.wind_speed);
        energy_spectrum = pierson_moskowitz(omega, peak_omega);
    }

    if (params.energy_spectrum == 1) {
        peak_omega = jonswap_peak_omega(chi, params.wind_speed);
        energy_spectrum = jonswap(omega, peak_omega, chi, params.peaking);
    }

    if (params.energy_spectrum == 2) {
        peak_omega = jonswap_peak_omega(chi, params.wind_speed);
        energy_spectrum = jonswap(omega, peak_omega, chi, params.peaking) * tma_correction(omega, depth);
    }

    float spread = direction_spectrum(theta, omega, peak_omega, params);

    return energy_spectrum * spread;
}

float short_wave_fade(float k_length, float fade_length) {
    return exp(-fade_length * fade_length * k_length * k_length);
}

#endif // OCEAN_WAVES_BASICS_INCLUDED