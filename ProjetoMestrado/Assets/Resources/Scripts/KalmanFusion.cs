using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalmanFusion : MonoBehaviour
{

    int [,] qMatrix, rMatrix, aMatrix, bMatrix;

    //estado estimado kalman anterior
    Vector3 eKant = new Vector3(0,0,0);

    //covariancia estado estimado pelo kalman anterior
    Vector3 pKant = new Vector3(0,0,0);

    //estado odometria x,y,teta da odometria;
    Vector3 eOdon = new Vector3(0,0,0);

    //estado recebido da camera -> que vem do tracking vuforia
    Vector3 eCam = new Vector3(0,0,0);

    //estado estimado a priori//
    Vector3 Epriori = new Vector3(0,0,0);

    //covariancia do estado a priori
    float [,] pKPriori = new float [3,3];

    void Start()
    {
        qMatrix = new int[,] {
            {1, 0, 0},
            {0, 1, 0},
            {0, 0, 1}
        };

        rMatrix = new int[,] {
            {1, 0, 0},
            {0, 1, 0},
            {0, 0, 1}
        };
    }

    void Update()
    {
        //Debug.Log(qMatrix*rMatrix);
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
}
