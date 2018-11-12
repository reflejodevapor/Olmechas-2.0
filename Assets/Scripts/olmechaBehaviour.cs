using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Olmechas{
	public class olmechaBehaviour : MonoBehaviour {

		public NavMeshAgent agent; //para detener el navigator.
		public List<Transform> selection = new List<Transform>();
		public Vector3 v3_target; //el vector del objetivo
		public Transform tr_target; //el transform del objetivo.

		private bool foundTarget = false;

		public int playerDamage = 2;

		public enum playerState{
			none, //esta es para que se quede paradito al inicio.
			attacking,
			moving,
			farming
		}

		public float timeBetweenAttacks = 3.0f; //tiempo entre cada putazo.


		playerState currentPlayerStatus; //pal enum.

		// Use this for initialization
		void Start () {
			tr_target = null;
			agent = this.gameObject.GetComponent<NavMeshAgent>();
			currentPlayerStatus = playerState.none;
		}
		
		// Update is called once per frame
		void Update () {
			Debug.Log(currentPlayerStatus.ToString()); //debugeo

			if(Input.GetKeyDown(KeyCode.Space)){
				currentPlayerStatus = playerState.moving; //testeo
			}

			if(currentPlayerStatus == playerState.attacking){
				if(tr_target != null){ //si si existe el transform del enemigo significa que no lo ha matado
					AttackObjective(tr_target);
				} else{
					currentPlayerStatus = playerState.moving; // Si si es nulo ps le dice que se mueva alv.
				}
				

			} else if(currentPlayerStatus == playerState.moving){
				if(foundTarget == false){ //este bool es para que la lista no se añada las mismas cosas una y otra vez.
					LookForTargets();
				} else{
					 /*Si ya hizo la lista y encontró al mas optimo y ya mataron al anterior, 
					 cuando se vuelva a ejecutar, borra la lista para ver de nuevo cuales están vivos aún
					 y vuelve a buscar.  */
					selection.Clear(); 
					LookForTargets(); 
				}
				
			} else if(currentPlayerStatus == playerState.farming){
				//quiero que haya modo atacar enemigos o modo farmear materiales. esto sería lo que haría aqui.
			}	

			if(agent.remainingDistance < 1.2f){ //en teoría debería cambiar a "atacar" solo cuando esté cerquita.
				currentPlayerStatus = playerState.attacking;
			}
			
		}
		
		void LookForTargets(){
			GameObject[] go = GameObject.FindGameObjectsWithTag("objective"); //busco los que tengan tag objetivo y los agrego a una lista los puros transforms.
			foreach(GameObject objetivo in go){
				selection.Add(objetivo.transform);
			}		
			tr_target = GetClosestEnemy(selection.ToArray()); //la funcion agarra un arreglo de transforms y te regresa el mas cercano.
			v3_target = new Vector3(tr_target.position.x,tr_target.position.y,tr_target.position.z); //el agente te pide un vector así que hago eso.
			agent.SetDestination(v3_target);
			foundTarget = true; //foundtarget es la variable para que la lista se busque una vez, si sirbe.
		}

		void AttackObjective(Transform _currentTarget){ //le mando el transform del que se va a chingar
			objectiveBehaviour obj = _currentTarget.GetComponent<objectiveBehaviour>();
			float nextActionTime = timeBetweenAttacks; //el clasico para repetir algo.. no sirve.
			if (Time.time >= nextActionTime ) {
			nextActionTime += Time.deltaTime;
				obj.health -= this.playerDamage; // pero si le baja al vida. SÚPER RAPIDO, pero si.
			}
		}

		public void ChangePlayerState(playerState _pState){ //esta fuincion es para que cambien el estado del personaje de fuera, no se puede cambiar directo.
			currentPlayerStatus = _pState; 
		}


		Transform GetClosestEnemy (Transform[] enemies) //me la tumbé de la primera respuesta en google.
		{
			Transform bestTarget = null;
			float closestDistanceSqr = Mathf.Infinity;
			Vector3 currentPosition = transform.position;
			foreach(Transform potentialTarget in enemies)
			{
				Vector3 directionToTarget = potentialTarget.position - currentPosition;
				float dSqrToTarget = directionToTarget.sqrMagnitude;
				if(dSqrToTarget < closestDistanceSqr)
				{
					closestDistanceSqr = dSqrToTarget;
					bestTarget = potentialTarget;
				}
			}
			return bestTarget;
		}

	}
}