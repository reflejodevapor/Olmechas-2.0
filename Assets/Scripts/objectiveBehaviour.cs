using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olmechas{
	public class objectiveBehaviour : MonoBehaviour {

		public int health = 6;
		public olmechaBehaviour ob;

		void Start () {
			ob = GameObject.FindGameObjectWithTag("Player").GetComponent<olmechaBehaviour>(); //checa el behaviour del player
		}
		
		// Update is called once per frame
		void Update () {
			if(this.health < 0){
				ob.ChangePlayerState(Olmechas.olmechaBehaviour.playerState.moving); //le dice que ya se ponga a buscar otra ves
				Destroy(this.gameObject); //bye
			}
		}
	}
}