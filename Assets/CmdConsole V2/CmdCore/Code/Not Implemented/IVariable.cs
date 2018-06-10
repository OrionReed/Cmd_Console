using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CmdConsole
{
    public interface IVariable
    {

    }

    public interface IVariable<T> : IVariable
    {
        T GetDefault();
        List<T> GetAll();
    }
}
