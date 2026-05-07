using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.Events;
public class Element
{

}

[System.Serializable]
public class GunElement : Element
{
    public UnityEvent OnReload;
    [SerializeField] private string _name;
    [SerializeField] private GameObject _gunModel;
    [SerializeField] private float _damage;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _ammunation;//muniçao total da arma para referencia pro jogador
    [SerializeField] private float _clipSize;
    [SerializeField] private float _reloadTime;//Quantidade de balas que o pente suportaa
    [SerializeField] private bool _hasScope;
    private float _ammunationClip;//Pente atual sendo utilizado até ter que puxar mais

    public GunElement(string name, float damage, float shootRate, float ammunation,float reloadTime, bool hasScope, GameObject gunModel)
    {
        _name = name;
        _damage = damage;
        _shootRate = shootRate;
        _ammunation = ammunation;
        _reloadTime = reloadTime;
        _hasScope = hasScope;
        _gunModel = gunModel;
    }
    public void Initialize() 
    {
        _ammunationClip = _clipSize;
        Debug.Log("inicio");
    }

    public bool UseAmmunation()
    {
        if (_ammunationClip <= 0)
        {
            if (_ammunation > 0)
            {
                OnReload.Invoke();
            }
            return false;
        }
        _ammunationClip--;
       return true;
    }
    public void Reload()
    {
        if (_ammunation <= 0)
            return;

        float ammunattionReload = _clipSize - _ammunationClip;
        if (ammunattionReload <= 0)
            return;
        if(_ammunation >= ammunattionReload)
        {
            ammunattionReload = _ammunation;
        }
        _ammunationClip += ammunattionReload;// >= ammunattionReload? ammunattionReload : _ammunation;
        _ammunation -= ammunattionReload;
    }



    public string Name { get => _name; }
    public float Damage { get => _damage;}
    public float ShootRate { get => _shootRate; }
    public float Ammunation { get => _ammunation;}
    public float ReloadTime { get => _reloadTime;}
    public bool HasScope { get => _hasScope;}
    public GameObject GunModel { get => _gunModel;}
}
[System.Serializable]
public class GunInventory
{
    [SerializeField] private List<GunElement> _gunsList;

    public List<GunElement> GunsList { get => _gunsList;}

    // array == fixo
    // arrays inventario visual 
    //armazenamento de referencia fixa 

    //  lista tamanho dinamico 
    // inimigos
    public void AddWeapon(GunElement newGun)
    {
        GunsList.Add(newGun);
    }
}
public class GunSystem : MonoBehaviour
{
    [SerializeField] private GunInventory _gunInventory;
    [SerializeField] private Transform _handGunModelParent;
    private Transform _camera;
    [SerializeField] private GunElement _handGun;
    private float _shootTimer;
    private bool _isReloanding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _handGun.Initialize();
        _camera = Camera.main.transform; 
        _shootTimer = _handGun.ShootRate;
        _handGun.OnReload.AddListener(() => StartCoroutine(Reload()));
        _gunInventory.AddWeapon(_handGun);
    }

    // Update is called once per frame
    void Update()
    {
        float currentGunIndex = Input.GetAxis("Mouse ScrollWheel");
        if (currentGunIndex != 0)
        {
            ChangeWeapon(currentGunIndex);
        }
        _shootTimer += Time.deltaTime;
        if (Input.GetButtonDown("Reload"))
        {
            if(_handGun.Ammunation <= 0)
            _handGun.OnReload.Invoke();
        }
        if (_isReloanding)
          return;
        print("timer");
        if (_shootTimer < _handGun.ShootRate)
            return;
        //verifica se atirou
        if (!Input.GetButtonDown("Fire1"))
            return;
        print("pressed");
        if (!_handGun.UseAmmunation())   
            return;

        //verifica se o player acertou algo
        if (!Physics.Raycast(_camera.position, _camera.forward, out RaycastHit target))
            return;
        //verifica se o objeto acertado implementa IShootable
        if (!target.collider.TryGetComponent(out IShootable shootable))
            return;
        //Aciona o método do contrato IShootable
        shootable.Hitted(_handGun.Damage, target.point);
        _shootTimer = 0;
    }

    private void ChangeWeapon(float nextIndex)
    {
        if (_gunInventory.GunsList.Count <= 1)
            return;

        int currentIndex = _gunInventory.GunsList.IndexOf(_handGun);
        currentIndex += (int)Mathf.Sign(nextIndex);

        if(currentIndex == _gunInventory.GunsList.Count)
        {
            currentIndex = 0;
        }

        else if (currentIndex < 0)
        {
            currentIndex = _gunInventory.GunsList.Count - 1;
        }
         
        _handGun = _gunInventory.GunsList[currentIndex];
        ChangeGunVisual();
    }

    IEnumerator Reload()
    {
        _isReloanding = true;
        //yield return new WaitUntil
        yield return new WaitForSeconds(_handGun.ReloadTime);
        _handGun.Reload();
        _shootTimer = _handGun.ShootRate;
        _isReloanding = false;
    }
    public void AddNewGun(GunElement newGun)
    {
        _handGun = newGun;
        _handGun.Initialize();
        _shootTimer = _handGun.ShootRate;
        _handGun.OnReload.AddListener(() => StartCoroutine(Reload()));
        _gunInventory.AddWeapon(newGun);
        ChangeGunVisual();
    }
    public void ChangeGunVisual()
    {
        Destroy(_handGunModelParent.GetChild(0).gameObject); //Remove o modelo da arma antiga
        GameObject gun = Instantiate(_handGun.GunModel, _handGunModelParent); //Instancia o modelo da nova arma
        gun.layer = LayerMask.NameToLayer("Gun");
        gun.transform.localPosition = new Vector3(0, 0, -gun.transform.localScale.z);
    }
}
