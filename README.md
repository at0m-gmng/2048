#2048
Данный проект представляет собой реализацию игры "2048".
В поле 4*4 генерируются 2 заполненые ячейки, с вероятностью 90% - 2, и с 10% - 4. Во время хода игрока ячейки с одинаковым номиналом объединяются, после чего в свободных ячейках происходит генерация новых значений. Номиналы 2 и 4 выделены тёмными цветами по задумке игры, остальные - белыми. Цвет ячеек меняется вместе с номиналом ячейки. 
- При достижении значения ячейки в 2048 считается, что игрок победил.
- Если никакие ячейки не могут объединиться и нельзя сгенерировать новые, то игрок проиграл.

Также ведётся подсчёт общего числа очков, и реализована возможность перезапуска игры.
<div align="center">
    <img src="generate_field.png" >
    <p>Начало игры и генерация игрового поля</p>
</div>
<div align="center">
    <img src="end_game_lose.png" >
    <p>Конец игры с результатом Lose</p>
</div>
