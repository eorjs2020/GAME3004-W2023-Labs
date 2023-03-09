using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public enum SelMode : int
{
    OnlyParent = 0,
    AndChildren = 1
}

[System.Serializable]
public enum AlphaType : int
{
    KeepHoles = 0,
    Intact = 1
}

[System.Serializable]
public enum OutlineMode : int
{
    Whole = 0,
    ColorizeOccluded = 1,
    OnlyVisible = 2
}


/*
 *  Not compatible with URP and HDRP at this moment.
    This function requires 3 full-screen-size rendertextures,and the outline shader contains for-loop.
    The cost of this is acceptable on PC,but if you are gonna use it on mobile platforms, you'd better optimize this by yourself.
*/
//[ExecuteInEditMode]
[System.Serializable]
[RequireComponent(typeof(Camera))]
public class SelectionOutline : MonoBehaviour
{
    
    public Material OutlineMat;
    public Shader OutlineShader, TargetShader;
    public RenderTexture Mask, Outline;
    public Camera cam;
    public CommandBuffer cmd;
    public bool Ini = false, Selected = false;
    public SelMode SelectionMode = SelMode.AndChildren;
    [Tooltip("The last two type will require rendering an extra Camera Depth Texture.")]
    public OutlineMode OutlineType = OutlineMode.ColorizeOccluded;
    [Tooltip("Decide whether the alpha data of the main texture affect the outline.")]
    public AlphaType AlphaMode = AlphaType.KeepHoles;
    public Renderer TargetRenderer, lastTarget;
    public Renderer[] ChildrenRenderers;
    [ColorUsageAttribute(true, true)]
    public Color OutlineColor = new Color(1f, 0.55f, 0f), OccludedColor = new Color(0.5f, 0.9f, 0.3f);
    [Range(0, 1)]
    public float OutlineWidth = 0.2f;
    [Range(0, 1)]
    public float OutlineHardness = 0.85f;

    void OnEnable()
    {

        Initialize();
    }
    public void Initialize()
    {
#if UNITY_WEBGL
        Shader.EnableKeyword("_WEBGL");
#endif
        OutlineShader = Shader.Find("Outline/PostprocessOutline");
        TargetShader = Shader.Find("Outline/Target");
        if (OutlineShader == null || TargetShader == null)
        {
            Debug.LogError("Can't find the outline shaders,please check the Always Included Shaders in Graphics settings.");
            return;
        }
        cam = GetComponent<Camera>();
        cam.depthTextureMode = OutlineType > 0 ? DepthTextureMode.None : DepthTextureMode.Depth;
        OutlineMat = new Material(OutlineShader);
        if (OutlineType > 0)
        {
            Shader.EnableKeyword("_COLORIZE");
            Mask = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.RFloat);
            Outline = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.RG16);
            if (OutlineType == OutlineMode.OnlyVisible)
                Shader.EnableKeyword("_OCCLUDED");
            else
                Shader.DisableKeyword("_OCCLUDED");

        }
        else
        {
            Shader.DisableKeyword("_OCCLUDED");
            Shader.DisableKeyword("_COLORIZE");
            Mask = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.R8);
            Outline = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.R8);
        }
        cam.RemoveAllCommandBuffers();
        cmd = new CommandBuffer { name = "Outline Command Buffer" };
        cmd.SetRenderTarget(Mask);
        cam.AddCommandBuffer(CameraEvent.BeforeImageEffects, cmd);
        Ini = true;
    }
    public void OnValidate()
    {
        if (!Ini)
        {
            Initialize();
        }
        cam.depthTextureMode = OutlineType > 0 ? DepthTextureMode.Depth : DepthTextureMode.None;
        if (OutlineType > 0)
        {
            Shader.EnableKeyword("_COLORIZE");

            if (OutlineType == OutlineMode.OnlyVisible)
                Shader.EnableKeyword("_OCCLUDED");
            else
                Shader.DisableKeyword("_OCCLUDED");

        }
        else
        {
            Shader.DisableKeyword("_OCCLUDED");
            Shader.DisableKeyword("_COLORIZE");
        }
    }
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (OutlineMat == null)
        {
            Initialize();
            if (!Ini)
                return;
        }
        OutlineMat.SetFloat("_OutlineWidth", OutlineWidth * 10f);
        OutlineMat.SetFloat("_OutlineHardness", 8.99f * (1f - OutlineHardness) + 0.01f);
        OutlineMat.SetColor("_OutlineColor", OutlineColor);
        OutlineMat.SetColor("_OccludedColor", OccludedColor);

        OutlineMat.SetTexture("_Mask", Mask);
        Graphics.Blit(source, Outline, OutlineMat, 0);
        OutlineMat.SetTexture("_Outline", Outline);
        Graphics.Blit(source, destination, OutlineMat, 1);
        //Graphics.Blit(Outline, destination);

    }
    public void RenderTarget(Renderer target)
    {
        Material TargetMat = new Material(TargetShader);
        bool MainTexFlag = false;
        string[] attrs = target.sharedMaterial.GetTexturePropertyNames();
        foreach (var c in attrs)
        {
            if (c == "_MainTex")
            {
                MainTexFlag = true;
                break;
            }
        }
        if (MainTexFlag && target.sharedMaterial.mainTexture != null && AlphaMode == AlphaType.KeepHoles)
        {
            TargetMat.mainTexture = target.sharedMaterial.mainTexture;
        }

        cmd.DrawRenderer(target, TargetMat);
        Graphics.ExecuteCommandBuffer(cmd);
    }
    public void SetTarget()
    {
        cmd.SetRenderTarget(Mask);
        cmd.ClearRenderTarget(true, true, Color.black);
        Selected = true;
        if (TargetRenderer != null)
        {
            RenderTarget(TargetRenderer);
            if (SelectionMode == SelMode.AndChildren && ChildrenRenderers != null)
            {

                foreach (Renderer c in ChildrenRenderers)
                {
                    if (c == TargetRenderer) continue;
                    RenderTarget(c);

                }
            }
        }
        else
        {
            Debug.LogWarning("No renderer provided for outline.");
        }
    }
    public void ClearTarget()
    {
        Selected = false;
        cmd.ClearRenderTarget(true, true, Color.black);

        Graphics.ExecuteCommandBuffer(cmd);
        cmd.Clear();
    }
   
}