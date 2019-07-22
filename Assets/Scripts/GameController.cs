using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Currency;

public class GameController : MonoSingleton<GameController>
{
    public CurrencyController CurrencyController { get; set; }

    public override void Init()
    {
        base.Init();
        CurrencyController = new CurrencyController();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CurrencyController?.Dispose();
        CurrencyController = null;
    }
}
