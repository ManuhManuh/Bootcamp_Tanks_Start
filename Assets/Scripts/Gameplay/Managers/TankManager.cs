﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Tanks
{
    public class TankManager : MonoBehaviour
    {
        private TeamConfig teamConfig;
        private TankMovement tankMovement;
        private TankShooting tankShooting;
        private TankHealth tankHealth;
        private GameObject canvasGameObject;

        private Player player;
        private PhotonView photonView;

        // TODO: Get player nickname
        public string ColoredPlayerName => $"<color=#{ColorUtility.ToHtmlStringRGB(teamConfig.color)}>Nickname</color>";
        public int Wins { get; set; }

        [PunRPC]
        public void OnHit(float explosionForce, Vector3 explosionSource, float explosionRadius, float damage)
        {
            tankMovement.GotHit(explosionForce, explosionSource, explosionRadius);
            tankHealth.TakeDamage(damage);
        }

        public void Awake()
        {
            SetupComponents();

            // TODO(DONE): Get team from photon
            player = photonView.Owner;
            teamConfig = FindObjectOfType<GameManager>().RegisterTank(this, 1);

            SetupRenderers();
        }

        private void SetupComponents()
        {
            photonView = GetComponent<PhotonView>();
            tankShooting = GetComponent<TankShooting>();
            tankHealth = GetComponent<TankHealth>();
            tankMovement = GetComponent<TankMovement>();
            canvasGameObject = GetComponentInChildren<Canvas>().gameObject;
        }

        private void SetupRenderers()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

            foreach (var meshRenderer in renderers)
                meshRenderer.material.color = teamConfig.color;
        }

        public void DisableControl()
        {
            tankMovement.enabled = false;
            tankShooting.enabled = false;

            canvasGameObject.SetActive(false);
        }

        public void EnableControl()
        {
            tankMovement.enabled = true;
            tankShooting.enabled = true;

            canvasGameObject.SetActive(true);
        }

        public void Reset()
        {
            transform.position = teamConfig.spawnPoint.position;
            transform.rotation = teamConfig.spawnPoint.rotation;

            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}