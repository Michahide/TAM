using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    //Range nya jangan diset 0
    [SerializeField, Range(0.01f, 1f)] float moveDuration =0.2f;

    [SerializeField, Range(0.01f, 1f)] float jumpHeight =0.5f;

    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel; }
    private int currentTravel_x;
    public bool IsDie { get => !this.enabled; }
    public int CurrentTravel_x { get => currentTravel_x; }

    public void Setup(int minZpos, int extent){
        backBoundary = minZpos - 1;
        leftBoundary = -(extent);
        rightBoundary = extent;
    }

    // Update is called once per frame
    private void Update()
    {
        //GetKey = Kalau dipencet terus, bakal kedeteksi terus
        //GetKeyDown = Kalau dipencet terus, cuma kedeteksi sekali
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     Debug.Log("Forward");
        // }

        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     Debug.Log("Back");
        // }

        //buat kontrol pencet panah atas, dll
        var moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDir += new Vector3(0, 0,1);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDir += new Vector3(0, 0,-1);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += new Vector3(1,0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir += new Vector3(-1, 0, 0);
        }

        /*dua if di bawah berguna agar karakter tidak dapat 
        move sebelum sampai di tempat*/
        if (moveDir == Vector3.zero)
            return;

        if(isJumping() == false)
            Jump(moveDir);

    }

    private void Jump(Vector3 targetDirection){
        var targetPosition = transform.position + targetDirection;

        /*Bikin gerakan langsung ngubah arah lihat hewan,
        misal maju ke depan lihat ke depan, gerak ke kanan, liht ke kanan*/
        transform.LookAt(targetPosition); 
        
        //Bikin sequence untuk animasi loncat ke atas
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight,moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0,moveDuration/2));

        if( targetPosition.z <= backBoundary ||
            targetPosition.x <= leftBoundary ||
            targetPosition.x >= rightBoundary||
            Coral.AllPositions.Contains(targetPosition)    
        )
            return;

        // if(  )
        //     return;
        // transform.DOMoveY(2f,0.2f)
        //     .OnComplete(()=>transform.DOMoveY(0,0.2f));
        
        //gerak maju/mundur/samping
        transform.DOMoveX(targetPosition.x,moveDuration/2);
        transform
            .DOMoveZ(targetPosition.z,moveDuration/2)
            .OnComplete(UpdateTravel);
    }

    private void UpdateTravel(){
        currentTravel = (int) this.transform.position.z;
        currentTravel_x = (int) this.transform.position.x;
        if(currentTravel > maxTravel)
            maxTravel = currentTravel;

        stepText.text = "STEP: " + maxTravel.ToString();
    }

    public bool isJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        //di execute sekali pada frame ketika nempel pertama kali
        
        /*GetComponent boros resource karena memeriksa 
        setiap anak/objek car, sebaiknya gunakan tag*/

        // var car = other.GetComponent<Car>();

        // if (car != null)
        // {

        // }

        if(this.enabled == false)
            return;

        if (other.tag == "Car")
        {
            AnimateCrash();
        }
    }

    //animasi ketika kena mobil
    private void AnimateCrash(/*Car car*/){
        //buat mental
        // var isRight = car.transform.rotation.y == 90;
        // transform.DOMoveX(isRight ? 8 : -8, 0.2f);
        // transform
        //      .DORotate(Vector3.forward*180, 10f)
        //      .SetLoops(100, LoopType.Restart);

        //buat gepeng
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(1, 0.2f);
        transform.DOScaleZ(1, 0.2f);
        this.enabled = false;
        dieParticles.Play();
    }

    private void OnTriggerStay(Collider other) {
        //di execute setiap frame selama masih nempel
    }

    private void OnTriggerExit(Collider other) {
        //di execute sekali pada frame ketika tidak nempel
    }
}
