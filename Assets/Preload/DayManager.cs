using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DayManager : MonoBehaviour
{
    //Variavel para o dia do jogo que o jogador parou.
    //Começa em 0 pois é a introdução.
    private int gameDay = 0;
    //Variavel para saber em que turno do dia o jogo está.
    //Se for 0 é manhã, se for 1 é tarde e se for 2 é noite.
    private int gameDayShift = 0;

    private void Start()
    {
        LoadDay();
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$day", gameDay);
    }

    //Metodo para pegar o dia do jogo
    public int GetDay()
    {
        return gameDay;
    }

    //Metodo para pegar o turno do dia em String
    public string GetStringDayShift() =>
    gameDayShift switch
    {
        0 => "manhã",
        1 => "tarde",
        2 => "noite",
        _ => throw new System.NotImplementedException()
    };

    //Metodo para pegar o turno do dia em inteiro
    public int GetIntDayShift()
    {
        return gameDayShift;
    }

    //Metodo chamado toda vez que passar um dia
    //E atualiza o turno do dia para manhã
    [YarnCommand("updateDay")]
    public void UpdateDay()
    {
        gameDay++;
        gameDayShift = 0;
    }

    //Metodo para atualizar o turno do dia
    //Se for de manha muda pra tarde e se for de tarde muda pra noite
    [YarnCommand("updateDayShift")]
    public void UpdateDayShift()
    {
        //Caso o turno do dia n seja noite ainda, então atualizar o turno
        if(gameDayShift < 2)
        {
            gameDayShift++;
        }
    }

    //Metodo para atualizar o dia salvo ao fechar o jogo
    public void LoadDay()
    {
        //Chamar de algum arquivo ou algo do tipo o dia em que o player salvo a ultima vez e atualizar na variavel de dia
    }
}
