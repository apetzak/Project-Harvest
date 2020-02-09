Shader "Masked/Mask2" {
	SubShader {
		// Render the mask after regular geometry, but before masked geometry and
		// transparent things.
 
		
 
		// Don't draw in the RGBA channels; just the depth buffer
 
		
		// Do nothing specific in the pass:
		Tags {"Queue" = "Geometry+2"}
		ColorMask 0
		ZWrite On
		Cull Back
		Pass {
			
		}
	}
}
