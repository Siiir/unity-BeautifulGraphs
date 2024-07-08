using UnityEngine;
using static UnityEngine.Mathf;
using System;

public static class FunctionLibrary
{
    public static Single Wave(Single x, Single z, Single t) =>  Sin(x +z+ t);

    public static Single MultiWave(Single x, Single z, Single t) => (  Sin(x +t/2)  +Sin(2*(z+t))/2  ) * (2f / 3f) +Sin(x+z+0.5f*t);

    public static float Ripple(Single x, Single z, Single t) => Sin(Sqrt(x*x+z*z)+t);

    public static float VFall(Single x, Single z, Single t) => Mathf.Abs(x) + 5 + -t % 10;




    public delegate Single FuncDeleg(Single x, Single z, Single t);
    public readonly static FuncDeleg[] Functions = { Wave, MultiWave, Ripple};
    public enum funcName { Wave, MultiWave, Ripple, VFall };
}