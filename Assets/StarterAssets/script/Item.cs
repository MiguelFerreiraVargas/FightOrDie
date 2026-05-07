using UnityEngine;

public abstract class Item : MonoBehaviour, Icollectable
{
    public abstract Element GetItem();
    /*
    //forla os filhos a implementarem
    //usado qnd tds os filhos usam, mas com comportamentos diferentes
    //nao declara corpo, apenas a assinatura
    protected abstract void Teste1();
    //metodos virtuais
    //permite q os filhos sobrescrevam, mas nao obriga
    //qnd apenas alguns dos filhos tem comportamentos diferente

    protected virtual void Teste2()
    {
         //corpo do metodo
    }
    //metodos normais
    // qnd tds os filhos tem o msm comportamento
    protected virtual void Teste3()
    {

    }*/
}
