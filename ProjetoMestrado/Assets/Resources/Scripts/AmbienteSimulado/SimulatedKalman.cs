using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

public class SimulatedKalman : MonoBehaviour
{

    public GameObject simulatedVirtualBot;
    public GameObject kalmanBot;
    public GameObject eCamBot;
    public GameObject eOdonBot;

    //outros scripts
    public SpeedControl speedControl;

    public float deltaT = 0.1f; //passo da operação

    //estado estimado kalman anterior
    public Vector<double> eKant = Vector<double>.Build.Dense(3, 0);

    double[] ekInicial = { 1.0, 1.0, 1.0 }; //só para preencher o vetor
    public Vector<double> eK;

    //covariancia estado estimado pelo kalman anterior
    Matrix<double> pKant = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0f);
    Matrix<double> pK = Matrix<double>.Build.DenseDiagonal(3, 3, 0.25f);

    // estado de inovação
    public Vector<double> yK = Vector<double>.Build.Dense(3, 0);

    //ganho de kalman
    Matrix<double> Kk = Matrix<double>.Build.DenseDiagonal(3, 3, 0);

    //estado odometria x,y,teta da odometria; x, y e angulo
    //colocar a odometria do virtual como teste
    //depois calcular a odemtria do real com base na velocidade das rodas
    public Vector<double> eOdon = Vector<double>.Build.Dense(3, 0);

    //estado recebido da camera -> que vem do tracking vuforia x y e angulo
    public Vector<double> eCam = Vector<double>.Build.Dense(3, 0);

    //estado estimado a priori//
    Vector<double> ePrior = Vector<double>.Build.Dense(3, 0);

    //covariancia do estado a priori
    Matrix<double> pkPrior = Matrix<double>.Build.DenseDiagonal(3, 3, 0);

    Matrix<double> S = Matrix<double>.Build.DenseDiagonal(3, 3, 0);

    //Matriz A é a matriz de transferencia do sistema(do passo anteiror ao seguinte)
    //na fusão sensorial ela vem da odometria//Matriz A é identidade
    Matrix<double> A = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);

    //Matriz H - matriz de observabilidade do sistema
    Matrix<double> H = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);

    //double[,] qInicial =  { { 0.05, 0.05, 0 }, { 0.05, 0.05, 0 }, {0, 0, 0.05 } };
    //Matrix<double> Q;
    Matrix<double> Q = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);

    //double[,] rInicial = { { 1.0, 0, 0 }, { 0, 1.0, 0 }, {0, 0, 1.0} }; //só para preencher o vetor
    Matrix<double> R = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);
    //matriz identidade
    Matrix<double> I = Matrix<double>.Build.DenseDiagonal(3, 3, 1.0);
    // Start is called before the first frame update
    void Start()
    {
        eK = Vector<double>.Build.DenseOfArray(ekInicial);
        //R = Matrix<double>.Build.DenseOfArray(rInicial);
        InvokeRepeating("CalculateKalmanFUsion", 0.1f, deltaT);
    }

    void CalculateKalmanFUsion()
    {

        //Debug.Log("Q = " + Q);
        //1 e 2)
        eKant = eK;
        pKant = pK;
        //3)
        eOdon[0] = eKant[0] + speedControl.forwardSpeed * Mathf.Cos((float)eKant[2]) * deltaT + Random.Range(-0.05f, 0.05f);
        eOdon[1] = eKant[1] + speedControl.forwardSpeed * Mathf.Sin((float)eKant[2]) * deltaT + Random.Range(-0.05f, 0.05f);
        eOdon[2] = eKant[2] + speedControl.angularSpeed * deltaT;

        //4)
        eCam[0] = simulatedVirtualBot.transform.position.x + Random.Range(-0.02f, 0.02f); //x da camera
        eCam[1] = simulatedVirtualBot.transform.position.z + Random.Range(-0.02f, 0.02f);  //y da camera
        eCam[2] = simulatedVirtualBot.transform.eulerAngles.y * Mathf.Deg2Rad; //teta da camera (em radianos)

        //5)
        ePrior = eOdon;
        //6)
        pkPrior = A * pKant * A.Transpose() + Q;

        //7)
        yK = eCam - (H * ePrior);

        //8)
        S = H * pkPrior * H.Transpose() + R;
        //S = H * pKant * H.Transpose() + R;

        //9)
        Kk = pkPrior * H.Transpose() * S.Inverse();
        //Kk = pKant * H.Transpose() + S.Inverse();

        //10)
        eK = eOdon + (Kk * yK);
        //11)
        pK = (I - Kk * H) * pkPrior;
        //pK = (I - Kk * H) * pkPrior * ((I - Kk * H).Transpose()) + (Kk * R * Kk.Transpose());

        MoveRobot(eK, kalmanBot);
        MoveRobot(eCam, eCamBot);
        MoveRobot(eOdon, eOdonBot);

        Debug.Log("eCam = " + eCam);
        Debug.Log("eOdon = " + eOdon);
        Debug.Log("KK = " + Kk);
        Debug.Log("S = " + S);
        Debug.Log("Ek = " + eK);
    }

    public void MoveRobot(Vector<double> state, GameObject go)
    {
        go.transform.position = new Vector3((float)state[0], 0, (float)state[1]);
        go.transform.rotation = Quaternion.Euler(0, (float)state[2] * Mathf.Rad2Deg, 0);
    }
}
