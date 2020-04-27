using System;

namespace XGame.Domain.Entities
{
    /**
     * Obter os jogos de cada jogador
    **/
    public class JogoPlataforma
    {
        public Guid Id { get; set; }

        public Jogo Jogo { get; set; }

        public Plataforma Plataforma { get; set; }

        public DateTime DataLancamento { get; set; }
    }
}
