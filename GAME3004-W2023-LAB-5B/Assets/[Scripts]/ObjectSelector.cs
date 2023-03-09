using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public Renderer renderer;
    public Material[] materialList;
    public SelectionOutline outline;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        materialList = renderer.materials;
        outline = FindObjectOfType<SelectionOutline>();
    }

    void OnMouseEnter()
    {

            Ray ray = outline.cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                outline.TargetRenderer = hit.transform.GetComponent<Renderer>();
                if (outline.lastTarget == null) outline.lastTarget = outline.TargetRenderer;
                if (outline.SelectionMode == SelMode.AndChildren)
                {
                    if (outline.ChildrenRenderers != null)
                    {
                        Array.Clear(outline.ChildrenRenderers, 0, outline.ChildrenRenderers.Length);
                    }
                    outline.ChildrenRenderers = hit.transform.GetComponentsInChildren<Renderer>();
                }


                if (outline.TargetRenderer != outline.lastTarget || !outline.Selected)
                {
                    outline.SetTarget();
                }
            //Debug.DrawRay(transform.position, hit.point - transform.position, Color.blue);
            outline.lastTarget = outline.TargetRenderer;
            }
            else
            {
                outline.TargetRenderer = null;
                outline.lastTarget = null;
                if (outline.Selected)
                {
                    outline.ClearTarget();
                }
            }
            //cmd.Blit(OutlineRT,)
        
        
    }

    void OnMouseOver()
    {
        foreach (var material in materialList)
        {
            material.SetColor("_Color", Color.green);
        }
    }

    void OnMouseExit()
    {
        foreach (var material in materialList)
        {
            material.SetColor("_Color", Color.white);
        }

        if (outline.Selected)
        {
            outline.ClearTarget();
        }
    }
}
