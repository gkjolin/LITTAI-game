﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShot : MonoBehaviour
{
	public int id = 0;
	public bool active = true;
	private bool isPressing_ = false;

	private ShotCharge charge_;
	public ShotCharge charge 
	{
		protected get { return charge_; }
		set { charge_ = value; }
	}

	private bool isDead_ = false;

	protected bool IsOwnEvent(int hwId)
	{
		return Parameters.GetMarkerId(hwId) == id;
	}

	protected bool CanUse(int energy)
	{
		return charge_.CanUse(energy);
	}

	protected bool Use(int energy)
	{
		return charge_.Use(energy);
	}

	protected bool IsDead()
	{
		return isDead_;
	}
	
	void Start()
	{
		SerialHandler.Pressed  += _OnPressed;
		SerialHandler.Pressing += _OnPressing;
		SerialHandler.Released += _OnReleased;
		OnStart();
	}

	protected virtual void OnStart()
	{
	}

	void OnDestroy()
	{
		if (charge_) charge_.Release(gameObject);
		SerialHandler.Pressed  -= _OnPressed;
		SerialHandler.Pressing -= _OnPressing;
		SerialHandler.Released -= _OnReleased;
		OnEnd();
	}

	void OnDisable()
	{
		if (charge_) charge_.Release(gameObject);
	}

	protected virtual void OnEnd()
	{
	}

	private void _OnPressed(int hwId)
	{
		if (IsOwnEvent(hwId) && active) {
			charge_.Get(gameObject);
			OnPressed();
			isPressing_ = true;
		}
	}

	protected virtual void OnPressed()
	{
	}
	
	private void _OnPressing(int hwId)
	{
		if (IsOwnEvent(hwId) && active) {
			if (isPressing_) {
				charge_.Get(gameObject);
				OnPressing();
			} else {
				_OnPressed(hwId);
			}
		}
	}

	protected virtual void OnPressing()
	{
	}

	private void _OnReleased(int hwId)
	{
		if (IsOwnEvent(hwId) && active) {
			OnReleased();
			charge_.Release(gameObject);
		}
	}

	protected virtual void OnReleased()
	{
	}

	public void OnPattern(PatternData pattern, Dictionary<int, GameObject> edgeMap)
	{
		List<GameObject> edges = new List<GameObject>();
		foreach (var id in pattern.ids) {
			if (!edgeMap.ContainsKey(id)) return;
			edges.Add(edgeMap[id]);
		}
		OnPattern(edges);
	}

	protected virtual void OnPattern(List<GameObject> edges)
	{
	}

	void OnDead()
	{
		isDead_ = true;
	}

	void OnRevival()
	{
		isDead_ = false;
	}
}