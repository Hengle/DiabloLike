    Shader "aRPG/OutlinedBumpSpec" {
        Properties {
            _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
            _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
            _Shininess ("Shininess", Range (0.01, 1)) = 0.01
            _MainTex ("Base (RGB)", 2D) = "white" {}
            _BumpMap ("Normalmap", 2D) = "bump" {}
            _OutlineColor ("Outline Color", Color) = (0,0,0,1)
            _Outline ("Outline width", Range (0.0000, 0.009)) = 0.005
        }
     
        SubShader {
            Tags { "RenderType"="Opaque" }
            UsePass "Bumped Specular/FORWARD"
            UsePass "Toon/Basic Outline/OUTLINE"
        }
       

        Fallback "Diffuse"
    }
