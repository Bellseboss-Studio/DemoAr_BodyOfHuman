shader "Custom/SolidColor"
{
    Properties
    {
        _Color("Tint", Color) = (1,1,1,1)
    }
    Subshader
    {
        Pass
        {
            CGPROGRAM
            //Pragmas
            #pragma vertex vertexShader
            #pragma fragment fragmentShader
            //Conect vars
            uniform fixed4 _Color;
            //Vertex input
            struct vertexInput
            {
                fixed4 vertex : POSITION;
            };
            //Vertex Output
            struct vertexOutput
            {
                fixed4 position : SV_POSITION;
                fixed4 color : COLOR;
            };
            //Vertex shader
            vertexOutput vertexShader(vertexInput i)
            {
                vertexOutput o;
                o.position = UnityObjectToClipPos(i.vertex);
                o.color = _Color;
                return o;
            }

            //fragment shader
            fixed4 fragmentShader(vertexOutput o) : SV_TARGET
            {
                return o.color;
            }
            ENDCG
        }
    }
    Fallback "Mobile/VertexLit"
}