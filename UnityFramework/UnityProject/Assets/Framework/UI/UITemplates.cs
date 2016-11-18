using UnityEngine;
using System.Collections.Generic;
using System;

public class UITemplates<T>
    where T : MonoBehaviour, ITemplatable
{
    bool foundTemplates = false;

    Dictionary<string, T> templates = new Dictionary<string, T>();

    Dictionary<string, T> cache = new Dictionary<string, T>();

    UnityEngine.Events.UnityAction<T> onCreateCallback;

    public UITemplates(UnityEngine.Events.UnityAction<T> onCreateCallback = null)
    {
        this.onCreateCallback = onCreateCallback;
    }

    public void FindTemplates()
    {
        foundTemplates = true;

        Resources.FindObjectsOfTypeAll<T>().ForEach(x =>
        {
            AddTemplate(x.name, x, replace: true);
            x.gameObject.SetActive(false);
        });
    }

    public void Delete()
    {
        templates.Clear();
        ClearCache();
        foundTemplates = false;
    }

    public void Delete(string name)
    {
        if (!ExistsTemplate(name))
            return;

        templates.Remove(name);
        ClearCache(name);
    }

    public void ClearCache()
    {
        foreach (var name in cache.Keys)
        {
           GameObject.DestroyImmediate(cache[name]);
        }
        cache.Clear();

    }

    public void ClearCache(string name)
    {
        if (!cache.ContainsKey(name))
        {
            return;
        }

        GameObject.DestroyImmediate(cache[name]);
        cache.Remove(name);
    }

    public bool ExistsTemplate(string name)
    {
        return templates.ContainsKey(name);
    }

    public void AddTemplate(string name, T template, bool replace = true)
    {
        if (ExistsTemplate(name))
        {
            if (!replace)
            {
                DebugConsole.LogError("Template with name '" + name + "' already exists.");
                return;
            }

            ClearCache(name);
            templates[name] = template;
        }
        else
        {
            templates.Add(name, template);
        }
        template.IsTemplate = true;
        template.TemplateName = name;
    }

    public T GetTemplate(string name)
    {
        if (!ExistsTemplate(name))
        {
            DebugConsole.LogError("Not found template with name '" + name + "'");
        }

        return templates[name];
    }

    public T GetDuplicate(string name)
    {
        if (!foundTemplates)
        {
            FindTemplates();
        }

        if ((!ExistsTemplate(name)) || (templates[name] == null))
        {
            DebugConsole.LogError("Not found template with name '" + name + "'");
        }

        T duplicate;
        if ((cache.ContainsKey(name)))
        {
            duplicate = cache[name];
        }
        else
        {
            duplicate = UnityEngine.Object.Instantiate(templates[name]) as T;

            duplicate.TemplateName = name;
            duplicate.IsTemplate = false;

            if (onCreateCallback != null)
            {
                onCreateCallback(duplicate);
            }
        }

        return duplicate;
    }

    public void ReturnCache(T instance)
    {
        instance.gameObject.SetActive(false);

        if (!cache.ContainsKey(instance.TemplateName))
        {
            cache.Add(instance.TemplateName, instance);
        }
        else
        {
            cache[instance.TemplateName] = instance;
        }
    }
}
