using System;

namespace WebApi.Enums
{
    public enum TypeCalculate
    {
        EEOnly = 1, //Se le suma solo al empleados y tiene un costo fijo independientemente de la aseguradora.
        AllMember = 2, //Se le suma a todos los miembros y tiene un costo fijo independientemente de la aseguradora.
        Tier = 3, //Se le calcular por el tipo de Tier y tiene costo diferente por aseguradora.
        AllMemberAndAge = 4, //Se le calcular por la edad para todos los miembros y tiene costo diferente por aseguradora.
        TierAndAge = 5 //Se le calcular por la edad y el tipo de Tier, y tiene costo diferente por aseguradora.

    }
}
