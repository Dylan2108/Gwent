using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope<T> : MonoBehaviour
{
   public Dictionary<string,T> Variables{get;}
   public Scope<T> scope{get;}
   public Scope()
   {
      Variables = new Dictionary<string, T>();
      scope = null;
   }
   public Scope(Scope<T> scope)
   {
     this.scope = scope;
     Variables = new Dictionary<string, T>();
   }
   public bool IsInScope(string name)
   {
      if(scope==null) return Variables.ContainsKey(name);
      else if(Variables.ContainsKey(name)) return true;
      else return scope.IsInScope(name);
   }
    public T Get(string name)
    {
       if(scope == null)
       {
         if(Variables.ContainsKey(name)) return Variables[name];
         else return default;
       }
       else if(Variables.ContainsKey(name)) return Variables[name];
       else return scope.Get(name);
    }
    public void Set(string name,T value)
    {
      if(!IsInScope(name) || Variables.ContainsKey(name)) Variables[name] = value;
      else scope.Set(name,value);
    }
}