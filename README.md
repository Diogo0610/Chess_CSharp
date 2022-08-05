# <h1 align="center"> Jogo de Xadrez no console usando C# </h1>

<h2 align="center"> Explicação do Projeto </h2>

<p> Criei esse jogo de Xadrex no console utilizando a linguagem C# como um exercício de um curso de C# realizado na Udemy, ministrado pelo Prof. Dr. 
Nélio Alves. Estou disponibilizando os códigos e o executável do jogo para quem tiver interesse em testar e ver como ficou!</p>

<p> Realizei alguns comentários nos códigos onde eu achei que uma breve explicação sobre determinado método seria pertinente, onde o nome não deixasse tão claro 
sua função.</p>

<p> Nos tópicos abaixo, estarei mostrando algumas jogadas e o funcionamento do jogo.</p>

<h3 align="center"> Jogada En Passant </h3>
<p> A jogada En Passant consiste em capturar um peão que, em seu primeiro movimento, avançou duas casas e alcançou a mesma linha do peão adversário. O peão
precisa ter se movimentado para uma coluna próxima, igual uma captura normal de um peão. As peças brancas podem aplicar o En Passant na linha 5 e as peças 
pretas aplicam na linha 4.</p>

![ezgif com-gif-maker](https://user-images.githubusercontent.com/70177817/182977954-4f140dca-b181-4da2-a656-70fa834ad49d.gif)

<h3 align="center"> Jogada Roque Maior e Roque Menor</h3>
<h4> Roque Maior </h4>
<p> A jogada Roque Maior consiste em movimentar o Rei e a Torre, ambas as peças não podem ter se movimentado na partida. Ocorre com a torre mais distante do 
Rei. Nessa jogada, o Rei avança duas casas em direção à Torre, enquanto essa avança três casas, invertendo a posição com o Rei.</p>

![ezgif com-gif-maker (1)](https://user-images.githubusercontent.com/70177817/182978752-c38cde00-fb27-4318-b6a7-e714583b7798.gif)

<h4> Roque Menor </h4>
<p> Essa jogada segue o mesmo conceito do Roque Maior, só que invés do Rei se movimentar em direção à Torre mais disntante, se movimenta para a mais próxima.
Ambas as peças se movimentam por duas casas.</p>

![ezgif com-gif-maker (2)](https://user-images.githubusercontent.com/70177817/182980891-a9a4b228-9d72-4c3f-9a6b-6c4a183dfd88.gif)

<h3 align="center"> Jogada Promoção</h3>
<p> Na jogada de promoção, o peão que alcançar a primeira ou a oitava linha será promovido automaticamente para Dama/Rainha.</p>

![ezgif com-gif-maker (4)](https://user-images.githubusercontent.com/70177817/182981359-8e265162-b638-4501-9ad7-07b9d65991c1.gif)

<h3 align="center"> Xeque</h3>
<p> Xeque é uma condição em que o Rei se encontra ameaçado, precisando realizar algum movimento para não ser capturado.</p>

![ezgif com-gif-maker (3)](https://user-images.githubusercontent.com/70177817/182981785-edb627c8-4074-4080-b0c6-a083ae4a34db.gif)

<h3 align="center"> Xeque Mate</h3>
<p> Xeque Mate é a última jogada da partida. O jogador que tem seu Rei em posição de Xeque Mate é o perdedor da partida. Nessa condição, 
o rei está encurralado e não há nenhum movimento possível que o impeça de ser capturado.</p>

![ezgif com-gif-maker (5)](https://user-images.githubusercontent.com/70177817/182982258-130076ab-6d4f-45c6-97dc-d891355bd2ca.gif)
