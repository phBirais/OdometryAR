using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

public class KalmanFusion : MonoBehaviour
{
    public GameObject virtualBot;

    public float deltaT = 0.1f; //passo da operação

    //Matriz H - matriz de observabilidade do sistema

    //estado estimado kalman anterior
   // Vector3 eK_= new Vector3(0,0,0);
   public Vector<double> eKant = Vector<double>.Build.Dense(3, 0);

    double[] ll = { 1.0, 2.0, 3.0}; //só para preencher o vetor
    public Vector<double> eK;

    //covariancia estado estimado pelo kalman anterior
    //Vector3 pK_ = new Vector3(0,0,0);
    Matrix<double> pKant = Matrix<double>.Build.DenseDiagonal(3, 3, 2.0);
    Matrix<double> pK = Matrix<double>.Build.DenseDiagonal(3, 3, 0);

    // estado de inovação
    public Vector<double> yK = Vector<double>.Build.Dense(3, 0);

    //ganho de kalman
    Matrix<double> Kk = Matrix<double>.Build.DenseDiagonal(3, 3, 0);

    //estado odometria x,y,teta da odometria; x, y e angulo
    //colocar a odometria do virtual como teste
    //depois calcular a odemtria do real com base na velocidade das rodas
    //Vector3 eOdon = new Vector3(0,0,0);
    public Vector<double> eOdon = Vector<double>.Build.Dense(3, 0);

    //estado recebido da camera -> que vem do tracking vuforia x y e angulo
    //Vector3 eCam = new Vector3(0,0,0);
    public Vector<double> eCam = Vector<double>.Build.Dense(3, 0);

    //estado estimado a priori//
    //Vector3 Eprior = new Vector3(0,0,0);
    Vector<double> ePrior = Vector<double>.Build.Dense(3, 0);

    //covariancia do estado a priori
    //float [,] pKPriori = new float [3,3];
    Matrix<double> pkPrior = Matrix<double>.Build.DenseDiagonal(3, 3, 0);

    Matrix<double> S = Matrix<double>.Build.DenseDiagonal(3, 3, 0);

    //Matriz A é a matriz de transferencia do sistema(do passo anteiror ao seguinte)
    //na fusão sensorial ela vem da odometria//Matriz A é identidade
    Matrix<double> A = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);
    Matrix<double> H = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);
    Matrix<double> Q = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);
    Matrix<double> R = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);

    //matriz identidade
    Matrix<double> I = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);


    void Start()
    {
        eK = Vector<double>.Build.DenseOfArray(ll);
        InvokeRepeating("CalculateMatrix", 0.1f, deltaT);
    }

    void Update()
    {
       
    }

    public void KallmanFusion()
    {
        /*
        //inicio do Kalman predict state
	        Eprior= Eodom//estado estimado a priori// aqui a gente usa a odometria como predição de estado
	        Pkprior= A*Pkant*transposta(A) + Q //covariancia do estado a priori
	        //update
	        yk= Ecam- H*Eprior; //estado de inovação
	        S=H*Pkprior*trasposta(H)+R; 
	        Kk= Pkprior*HT*inversa(S)// ganho Kalman
	        Ek= Ekant+ K*yk;
	        Pk = (I-Kk*H)*Pkprior; 
      return (Ek, Pk);
         */
    }

    void CalculateMatrix()
    {
        //1 e 2)
        eKant = eK;
        pKant = pK;
        //3)
        eOdon[0] = eOdon[0] + 1.0;//X da odometria
        eOdon[1] = eOdon[1] + 1.0; //y da odometria
        eOdon[2] = eOdon[2] + 1.0;
        /*
        eOdon[0] = virtualBot.transform.position.x;//X da odometria
        eOdon[1] = virtualBot.transform.position.y; //y da odometria
        eOdon[2] = virtualBot.transform.eulerAngles.y * Mathf.Deg2Rad;//teta da odometria (em radianos)
        */
        //4)
        eCam[0] = eCam[0] + 2.0f;  //x da camera
        eCam[1] = eCam[1] + 2.0f; //y da camera
        eCam[2] = eCam[2] + 2.0f; //teta da camera (em radianos)
        //5)
        ePrior = eOdon;
        //6)
        pkPrior = A * pKant * A.Transpose() + Q;
        
        //7)
        yK = eCam - (H * ePrior);
      
        //8)
        S = H * pkPrior * H.Transpose() + R;
      
        //9)
        Kk = pkPrior * H.Transpose() + S.Inverse();
       
        //10)
        eK = eOdon + (Kk * yK);
        //11)
        pK = (I - Kk * H) * pkPrior;

        Debug.Log("eCam = " + eCam);
        Debug.Log("eOdon = " + eOdon);
        Debug.Log("yK = " + yK);
        Debug.Log("Ek = " + eK);
      
    }

    public void StopMatrixCalculation()
    {
        CancelInvoke();
    }
}
