shader_type canvas_item;

uniform vec4 col: source_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
	COLOR = texture(TEXTURE, UV);
	COLOR *= col;
	
	COLOR.a = 0.9;
	
	if (UV.x > 0.95 || UV.y > 0.9) {
		COLOR.r += 0.005;
		COLOR.b = 0.005;
		COLOR.g += 0.005;
		COLOR.a = 0.9;
	} else if (UV.x < 00.53 || UV.y < 0.09) {
		COLOR.r += 0.05;
		COLOR.b = 0.05;
		COLOR.g += 0.05;
		COLOR.a = 0.1;
	}
	
}
