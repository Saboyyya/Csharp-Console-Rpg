using System;

class Program
{
    static void Main()
    {
        Player player = new Player(100);
        Character enemy = EnemyFactory.CreateRandomEnemy();

        Console.WriteLine($"⚔️ Um {enemy.GetType().Name} apareceu!");

        while (player.Vida > 0 && enemy.Vida > 0)
        {
            Console.WriteLine("\n1 - Atacar | 2 - Fugir");
            Console.Write("Digite a opção escolhida: ");

            if (!int.TryParse(Console.ReadLine(), out int escolha))
            {
                Console.WriteLine("Entrada inválida!");
                continue;
            }

            switch (escolha)
            {
                case 1:
                    int danoPlayer = player.Attack();
                    enemy.TakeDamage(danoPlayer);

                    Console.WriteLine($"Você causou {danoPlayer} de dano.");
                    Console.WriteLine($"Vida do inimigo: {enemy.Vida}");

                    if (enemy.Vida <= 0)
                    {
                        Console.WriteLine("🎉 Inimigo derrotado!");
                        return;
                    }

                    int danoEnemy = enemy.Attack();
                    player.TakeDamage(danoEnemy);

                    Console.WriteLine($"Inimigo atacou e causou {danoEnemy} de dano.");
                    Console.WriteLine($"Sua vida: {player.Vida}");

                    if (player.Vida <= 0)
                    {
                        Console.WriteLine("💀 Você foi derrotado.");
                        return;
                    }
                    break;

                case 2:
                    Console.WriteLine("Você fugiu da batalha!");
                    return;

                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }
}

abstract class Character
{
    public int Vida { get; protected set; }
    protected int danoBase;
    protected int chanceCritico;
    protected double multiplicadorCritico;
    protected static Random random = new Random();

    public virtual int Attack()
    {
        bool critico = random.Next(1, 101) <= chanceCritico;
        int danoFinal = danoBase;

        if (critico)
        {
            danoFinal = (int)(danoBase * multiplicadorCritico);
            Console.WriteLine("Ataque crítico!");
        }

        return danoFinal;
    }

    public void TakeDamage(int dano)
    {
        Vida -= dano;
        if (Vida < 0)
            Vida = 0;
    }
}

static class EnemyFactory
{
    private static Random random = new Random();

    public static Character CreateRandomEnemy()
    {
        int escolha = random.Next(0, 2);

        if (escolha == 0)
            return new Goblin();
        else
            return new Orc();
    }
}

class Player : Character
{
    public Player(int vida)
    {
        Vida = vida;
        danoBase = 10;
        chanceCritico = 20;
        multiplicadorCritico = 2.0;
    }
}

class Enemy : Character
{
    public Enemy(int vida, int dano)
    {
        Vida = vida;
        danoBase = dano;
        chanceCritico = 10;
        multiplicadorCritico = 1.5;
    }
}

class Goblin : Enemy
{
    public Goblin() : base(100, 5) { }

    public override int Attack()
    {
        Console.WriteLine("🗡️ O Goblin ataca com fúria!");
        return base.Attack();
    }
}

class Orc : Enemy
{
    public Orc() : base(150, 12) { }

    public override int Attack()
    {
        Console.WriteLine("👹 O Orc desfere um golpe brutal!");
        return base.Attack();
    }
}
