Asset Creator - Vladislav Horobets (ErbGameArt).
-----------------------------------------------------

If you want to use post-effect like in the demo video:

1) Download Post Effects throw Package manager end enable "Bloom". Or you can use any other "Bloom".
   There are a couple of free ones at Asset Store.
   You can also create your own "Bloom" effect: https://catlikecoding.com/unity/tutorials/advanced-rendering/bloom/
2) You should turn on "HDR" on main camera for correct post-effects. (bloom post-effect works correctly only with HDR)
If you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "MSAA of post effect". 
It's faster then default MSAA and have the same quality.

For HDRP and LWRP:
Download Post Processing Stack from Package manager.

You can also use your own "Bloom".


2) Shaders
2.1)The "Use depth" on the material from the custom shaders is the Soft Particle Factor.
2.2)Use "Center glow"[MaterialToggle] only with particle system. This option is used to darken the main texture with a white texture (white is visible, black is invisible).
    If you turn on this feature, you need to use "Custom vertex stream" (Uv0.Custom.xy) in tab "Render". And don't forget to use "Custom data" parameters in your PS.
2.3)The distortion shader only works with standard rendering. Delete (if exist) distortion particles from effects if you use LWRP or HDRP!
2.4)You can change the cutoff in all shaders (except Add_CenterGlow and Blend_CenterGlow ) using (Uv0.Custom.xy) in particle system.

3)Linear color space + HDRP
3.1)Dissable soft particles or uncheck checkbutton "Use Depth" in all materials if you use HDRP.
3.2)Reduce Mask clip value in all materials from Blend_TwoSides shader if you use Linear color space.
    You can find all materials by searching ts and twoSides

4)For HDRP From Unity 2019.2
  To display effects properly, turn off the "Use depth" setting in all materials 
  and increase the "Custom data: x" value in particle systems.

  SUPPORT ASSET FOR URP(LWRP) or HDRP here --> https://assetstore.unity.com/packages/slug/157764
  SUPPORT ASSET FOR URP(LWRP) or HDRP here --> https://assetstore.unity.com/packages/slug/157764
  SUPPORT ASSET FOR URP(LWRP) or HDRP here --> https://assetstore.unity.com/packages/slug/157764


Contact me if you have any problems or questions.
My email: gorobecn2@gmail.com