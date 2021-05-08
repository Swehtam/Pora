using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    //Variavel para o dia do jogo que o jogador parou.
    //Come�a em 1 pois � o primeiro dia.
    private static int gameDay = 0;
    //Variavel para saber em que turno do dia o jogo est�.
    //Se for 0 � manh�, se for 1 � tarde e se for 2 � noite.
    private static int gameDayShift = 1;

    private void Start()
    {
        LoadDay();
    }

    //Metodo para pegar o dia do jogo
    public static int GetDay()
    {
        return gameDay;
    }

    //Metodo para pegar o turno do dia em String
    public static string GetStringDayShift() =>
    gameDayShift switch
    {
        0 => "manh�",
        1 => "tarde",
        2 => "noite",
        _ => throw new System.NotImplementedException()
    };

    //Metodo para pegar o turno do dia em inteiro
    public static int GetIntDayShift()
    {
        return gameDayShift;
    }

    //Metodo chamado toda vez que passar um dia
    //E atualiza o turno do dia para manh�
    public static void UpdateDay()
    {
        gameDay++;
        gameDayShift = 0;
    }

    //Metodo para atualizar o turno do dia
    //Se for de manha muda pra tarde e se for de tarde muda pra noite
    public static void UpdateDayShift()
    {
        //Caso o turno do dia n seja noite ainda, ent�o atualizar o turno
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
