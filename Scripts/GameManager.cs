using UnityEngine;
using System.Collections;

enum Action : byte {NOOP, ATK, DEF, REL}

// Recebe as mensagens do player e do inimigo conectado, avalia o resultado nesse turno e envia o resultado calculado para os dois
public class GameManager : MonoBehaviour {

	// metodo para receber/enviar mensagem do/ao player, metodo para receber/enviar mensagem do/ao inimigo
	// criar scripts auxiliares(Ex.: AnimationController) para nao sobrecarregar o GameManager

	public Player p1, p2;
	public Timer timer;
	public Connection connect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
