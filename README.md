Relatório do Projeto: Only Up - Jogo 2D em MonoGame
Objetivo do Jogo
O jogo Only Up é um jogo 2D de plataformas verticais onde o objetivo principal do jogador é subir o mais alto possível saltando de plataforma em plataforma, evitando cair. A mecânica central baseia-se na precisão dos saltos e na gestão do tempo, pois cada vez que o personagem toca numa plataforma, esta cai passados 2 segundos, tornando a jogabilidade mais desafiadora.

O jogador perde o jogo se cair abaixo do ecrã. Nesse caso, é apresentado um ecrã de "Game Over" e o jogador pode recomeçar pressionando a tecla R.

Recursos Utilizados
Linguagem: C#

Framework: MonoGame 3.8

Plataforma alvo: Windows Desktop

Estreutura do Projeto:
OnlyUp/
├── Content/
│ ├── background.png
│ ├── platform_wood.png
│ ├── player1.png
│ ├── player2.png
│ ├── player3.png
│ ├── jump.wav
│ ├── death.wav
│ ├── gameover.png
│ └── GameFont.spritefont (não usado via pipeline)
├── Game1.cs
├── OnlyUp.csproj
└── README.md


Assets:

Sprites do jogador (player1.png, player2.png, player3.png)

Fundo do jogo (background.png)

Textura das plataformas (platform_wood.png)

Imagem de "Game Over" (gameover.png)

Sons: salto (jump.wav), morte (death.wav)

Controlo do Jogador
Seta esquerda/direita: Move o personagem lateralmente.

Barra de espaço: Faz o personagem saltar (se estiver no chão).

Tecla R: Reinicia o jogo após a morte.

Tecla ESC: Fecha o jogo.

Comportamento das Plataformas
As plataformas são geradas de forma procedural, ou seja, à medida que o jogador sobe, novas plataformas são automaticamente criadas acima dele, com posições horizontais aleatórias. Quando o jogador pisa numa plataforma, um temporizador de 2 segundos é iniciado. Após esse tempo, a plataforma desaparece (simulando uma queda), aumentando a dificuldade do jogo.

Plataformas que ficam muito abaixo do jogador são removidas para manter o desempenho.

Estados do Jogo
O jogo tem dois estados principais:

Estado de jogo normal: O jogador pode mover-se e subir.

Estado de "Game Over": Ativado quando o jogador cai abaixo da janela. Exibe uma imagem com a mensagem de derrota. Pressionar R reinicia o jogo.

Animação do Jogador
O jogador está animado com 3 sprites diferentes, alternados ao longo do tempo (ciclo simples de animação). Isso dá a sensação de movimento enquanto ele se desloca pelo ecrã.

Explicação do ficheiro Game1.cs
O ficheiro Game1.cs contém toda a lógica do jogo. Está estruturado da seguinte forma:

1. Declaração de variáveis
Carrega texturas, sons, e define propriedades como gravidade, velocidade de salto, largura e altura de plataformas e do jogador.

2. Método Initialize()
Define o tamanho da janela (480x800).

Inicializa o jogo com uma plataforma inicial e a posição do jogador.

3. Método LoadContent()
Carrega imagens e sons diretamente do sistema de ficheiros, sem usar o MonoGame Pipeline (FromStream).

Carrega a imagem de fundo, sprites do jogador, plataformas, sons e o ecrã de Game Over.

4. Método Update()
Controla:

Input do jogador.

Física (gravidade e salto).

Colisão com plataformas.

Geração procedural de novas plataformas.

Animação do personagem.

Detecção de queda (Game Over).

Delay para a queda das plataformas após serem tocadas.

5. Método Draw()
Desenha:

Fundo.

Plataformas ativas.

Jogador animado.

Imagem de "Game Over" (caso aplicável).

6. Função RestartGame()
Reposiciona o jogador.

Limpa as plataformas anteriores.

Reinicia o estado do jogo.

Conclusão
Este projeto demonstra uma aplicação prática de conceitos aprendidos na disciplina de Técnicas de Desenvolvimento de Jogos, como:

Leitura de recursos externos.

Lógica de física simples.

Colisões em 2D.

Animação básica.

Gestão de estados de jogo.

Criação de desafios crescentes com plataformas que desaparecem.

O jogo está funcional, é desafiante, e pode ser facilmente expandido com pontuação, música de fundo, menus ou níveis.

