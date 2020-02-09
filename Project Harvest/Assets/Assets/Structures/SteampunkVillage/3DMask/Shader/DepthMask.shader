Shader "Masked/Mask" {
	SubShader {
		// Render the mask after regular geometry, but before masked geometry and
		// transparent things.
 
		
 
		// Don't draw in the RGBA channels; just the depth buffer
 
		
		// Do nothing specific in the pass:
		Tags {"Queue" = "Geometry+4"}
		ColorMask 0
		ZWrite On
		Cull Back
		Pass {
			
		}
	}
}
