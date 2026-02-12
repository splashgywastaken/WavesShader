#ifndef FLOW_MAP_CUSTOM_FUNCTIONS
#define FLOW_MAP_CUSTOM_FUNCTIONS

#include "Assets/Shaders/CustomFunctions/helper_custom_functions.hlsl"

float3 flow_uvw_float(
    float2 uv,
    float2 flow_vector, float flow_offset, 
    float2 jump, float2 jump_tiling,
    float time, bool use_phase_offset
    ) {
    // Использование дополнительного сдвига
    // для повторных вычислений текстурных координат
    float phase_offset = use_phase_offset ? 0.5 : 0;
    // Значение определяющее цикл анимации,
    // берём целую часть для получения цикличной анимации 
    float progress = frac(time + phase_offset);
    float3 uvw;
    // Сдвиг координат с учетом кадра анимации,
    // а также параметров сдвига и направления сдвига
    uvw.xy = uv - flow_vector * (progress + flow_offset);
    // Добавление тайлинга
    uvw.xy *= jump_tiling;
    // Дополнительный сдвиг
    uvw.xy += phase_offset;
    // Добавление сдвига текстурных координат в виде прыжка
    uvw.xy += (time - progress) * jump;
    // Восстановление z координаты в соответствии с кадром анимации
    uvw.z = 1 - abs(1 - 2 * progress);
    return uvw;
}

void apply_flow_to_texture_float(
    float4 color,
    UnityTexture2D flow_tex, UnityTexture2D derivative_height_map, UnityTexture2D main_tex,
    float flow_strength,
    float flow_offset,
    float height_scale_constant,
    float height_scale_modulated,
    float2 jump,
    float jump_tiling,
    float2 uv,
    float time,
    float animation_speed,
    out float4 result_color,
    out float3 normal
) {
    // Семплирование значений из текстуры хранящей вектор движения, скорость
    float3 flow_sampled = sample_texture2d_float(flow_tex, uv).xyz;
    // Восстановление значений для векторов движения из-за особенностей сжатия текстуры
    flow_sampled.xy = flow_sampled.xy * 2 - 1;
    // Усиление значений вектора движения и скорости через параметр flow_strength
    flow_sampled *= flow_strength;
    // sample_texture2d_float(flow_tex, uv) - Получение шума Перлина из текстуры
    float noised_time = time * animation_speed + sample_texture2d_float(flow_tex, uv).w;

    // Получение векторов текстурных координат для 
    float3 uvw_a
    = flow_uvw_float(uv, flow_sampled.xy, flow_offset, jump, jump_tiling, noised_time, false);
    float3 uvw_b
    = flow_uvw_float(uv, flow_sampled.xy, flow_offset, jump, jump_tiling, noised_time, true);

    // Получение итогового цвета изображения путем семплирования с использованием текстурных координат
    // Семплирование с использованием текстурных координат без дополнительного сдвига
    result_color =  sample_texture2d_float(main_tex, uvw_a.xy) * uvw_a.z;
    // Семплирование с использованием текстурных координат с дополнительным сдвигом
    result_color += sample_texture2d_float(main_tex, uvw_b.xy) * uvw_b.z;
    // Домножение итогового значения на параметр определяющий цвет поверхности
    result_color *= color;

    // Расчёты для нормалей
    // Задаёт множитель высоты как переменную основанную на скорости потока 
    // Таким образом для более сильного потока получаются более высокие волны
    // а для слабого потока получаются низкие волны 
    float height_scale = flow_sampled.z * height_scale_modulated + height_scale_constant;
    // Распаковка значений карты высот и производных высоты 
    float3 derivative_height_a
        = unpack_derivative_height_float(derivative_height_map, uvw_a.xy) * uvw_a.z * height_scale;
    float3 derivative_height_b
        = unpack_derivative_height_float(derivative_height_map, uvw_b.xy) * uvw_b.z * height_scale;
    // Комбинирование значений производных
    normal = normalize(float3(-(derivative_height_a.xy + derivative_height_b.xy), 1));
}

#endif // FLOW_MAP_CUSTOM_FUNCTIONS
