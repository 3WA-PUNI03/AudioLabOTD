using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] InputActionReference _move;
    [SerializeField] InputActionReference _look;
    [SerializeField] CharacterController _controller;
    [SerializeField] float _speed;

    [SerializeField] Transform _cameraTransform;

    [SerializeField] Vector2 _cameraSensibility;

    [SerializeField] CinemachineVirtualCamera _vc;

    float _vertical;
    float _horizontal;

    private void Update()
    {
        MoveCharacter();

        // R�cup la direction Look
        Vector2 look = _look.action.ReadValue<Vector2>();

        // On change notre rotation par rapport � Looks
        _horizontal += look.x * _cameraSensibility.x;
        _vertical -= look.y * _cameraSensibility.y;

        // On clamp l'axe vertical pour pas faire de looping
        _vertical = Mathf.Clamp(_vertical, -80, 80);
        
        // On applique la rotation droite/gauche � notre objet pour tourner tout le monde
        transform.rotation = Quaternion.Euler(0, _horizontal, 0);
        // On applique la rotation haut/bas uniquement � notre camera pour qu'elle tourne seulesss
        _cameraTransform.localRotation = Quaternion.Euler(_vertical, 0, 0);
        
    }

    private void MoveCharacter()
    {
        // On r�cup�re la direction du joystick
        Vector2 joystick = _move.action.ReadValue<Vector2>();

        // La direction Y du joystick on veut que �a bouge le Z de notre personnage, donc on pr�pare un vector3 
        // qui prend le Y comme direction Z.
        Vector3 direction = new Vector3(joystick.x, 0, joystick.y);

        // On lui applique une vitesse
        direction *= _speed;

        direction = _controller.transform.TransformDirection(direction);

        // On envoi au CharacterController
        _controller.Move(direction);
    }
}
