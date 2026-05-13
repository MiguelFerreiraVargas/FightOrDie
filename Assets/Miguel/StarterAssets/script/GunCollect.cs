using UnityEngine;

public class GunCollect : Item, Icollectable
{
    [SerializeField] private GunElement _attributes;

    public override Element GetItem()
    {
        Destroy(gameObject);
        return _attributes;
    }

    /*
    protected override void Teste1()
    {
        throw new System.NotImplementedException();
    }
     se eu sobrescrevo o metodo virtual do pai
     chamar no filho, o metodo do filho é executado
 
    protected override void Teste2()
    {
        Debug.Log("Teste2");
    }
    private void Start()
    {
        Teste2();
        Teste3();
    }
    */
}
