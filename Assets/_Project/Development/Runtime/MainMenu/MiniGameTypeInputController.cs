#region

using System;
using Runtime.Utility.Controllers;
using UnityEngine;

#endregion

namespace Runtime.MainMenu
{
    public class MiniGameTypeInputController : Controller
    {
        // я вот чет пока писал этот класс подумал, а насколько лучше будет разделить это класс на два
        // и вынести каждый инпут в свой класс и сделать под него какой-то абстрактный класс, 
        // который будет чисто за инпут одной кнопки отвечать

        // условно какой-нибудь класс, в конктрукторе ты передаешь KeyCode и потом получаешь event,
        // когда он нажат? И методы включения-выключения уже есть т.к. он контроллер

        public event Action NumbersTypeChosen;
        public event Action LettersTypeChosen;

        private const KeyCode NumbersTypeKeyCode = KeyCode.Alpha1;
        private const KeyCode LettersTypeKeyCode = KeyCode.Alpha2;

        protected override void ProcessUpdateInternal(float deltaTime)
        {
            if (Input.GetKeyDown(NumbersTypeKeyCode))
                NumbersTypeChosen?.Invoke();

            if (Input.GetKeyDown(LettersTypeKeyCode))
                LettersTypeChosen?.Invoke();
        }
    }
}