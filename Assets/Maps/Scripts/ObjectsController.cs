using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;

public class ObjectsController : MonoBehaviour
{
    [Serializable]
    public struct SpawnVariables
    {
        /// <summary>
        /// O nome da variavel para spawnar o objeto
        /// </summary>
        /// <remarks>
        /// Lembrar de incluir o prefixo `$` nas variaveis.
        /// </remarks>
        public string name;
        /// <summary>
        /// O valor da variavel, em string.
        /// </summary>
        /// <remarks>
        /// Essa string será convertida para o tipo apropriado
        /// dependendo do valor de <see cref="type"/>.
        /// </remarks>
        public string value;

        /// <summary>
        /// O tipo da variavel.
        /// </summary>
        public Yarn.Value.Type type;
    }

    [Tooltip("Se alguma dessas variaveis condizer com a condição então spawna.")]
    public SpawnVariables[] arraySpawnVariables;

    private InMemoryVariableStorage inMemoryVariableStorage;

    void Start()
    {
        //Pega a classe que vê as variaveis na memoria
        if (inMemoryVariableStorage == null)
            inMemoryVariableStorage = InstancesManager.singleton.GetInMemoryVariableStorage();

        if (arraySpawnVariables.Length > 0)
        {
            bool spawn = CheckSpawnVariables();
            if (!spawn)
                gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Metodo para verificar as variaveis
    /// </summary>
    /// <returns>
    /// Se alguma variavel condizer com a condição então retura TRUE;
    /// Caso contrario retorna FALSE
    /// </returns>
    private bool CheckSpawnVariables()
    {
        bool spawn = false;

        foreach (SpawnVariables sv in arraySpawnVariables)
        {
            //Pega a variavel se existir,
            //Se nao existir vai vir null, ou seja tipo diferente, então nao precisa comparar os valores
            Yarn.Value memoryValue = inMemoryVariableStorage.GetValue(sv.name);

            //Se não existir a variavel então retorna que não pode spawnar
            if (memoryValue == null)
            {
                continue;
            }

            if (sv.type == memoryValue.type)
            {
                string stringValue = "";
                stringValue = sv.type switch
                {
                    Yarn.Value.Type.Bool => memoryValue.AsBool.ToString(),
                    Yarn.Value.Type.Number => memoryValue.AsNumber.ToString(),
                    Yarn.Value.Type.String => memoryValue.AsString,
                    _ => "<unknown>",
                };

                if (stringValue.Equals(sv.value))
                {
                    spawn = true;
                    break;
                }
            }
            else
            {
                continue;
            }
        }

        return spawn;
    }
}
