


using System;

public static class SaveSystem
{
    // metodo para crear un nuevo archivo de guardado

    // metodo para cargar un nuevo archivo de guardado

        // el archivo de guardado tiene uqe tener : 
            // *** INFORMACION DE LA SESION (playerPrefs)
            // *** ESTADISTICAS BASICAS DEL GAMEPLAY DEL JUGADOR
            // *** OBJETOS RECOLECTADOS POR EL JUGADOR
            // *** NIVEL (ESCENA) Y POSICION EN EL MUNDO DONDE GUARDO LA PARTIDA ANTERIOR



    // load desde los archivos del juego los stats del jugador
    public static UnitStats LoadPlayerUnitStats()
    {
        UnitStats stats = new(
            health:5, 
            attack:4, 
            defense:8, 
            maxMovementSpeed:2, 
            attackRange:1);
        return stats;
    }

}
