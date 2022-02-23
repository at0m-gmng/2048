using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

//висит на объекте который создаётся и перемещается (анимация)
public class CellAnimation : MonoBehaviour
{
    // для идентичности базовой плитке будем изменять значение очков и цвет
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI points;

    // для длительности анимации, перемещения и появления
    private float moveTime = .1f;
    private float appearTime = .1f;

    private Sequence sequence; // переменная для очереди

    // показывает анимация при перемещении из одной плитки в другую
    public void Move(Cell from, Cell to, bool isMerging)
    {
        from.CancelAnimation(); //останавливаем анимацию у плитки, из которой двигаемся;
        to.SetAnimation(this);

        //маскируем объект анимации под ячейку
        image.color = ColorManager.Instance.CellColor[from.Value];
        points.text = from.Points.ToString();
        points.color = from.Value <= 2 ? ColorManager.Instance.PointsDarkColor : ColorManager.Instance.PointsLightColor;

        //перемещаемся в позицию ячейки, из которой выезжаем
        transform.position = from.transform.position;

        sequence = DOTween.Sequence(); // инициализируем очередь и добавляем перемещение в позицию будущей плитки
        sequence.Append(transform.DOMove(to.transform.position, moveTime).SetEase(Ease.InOutQuad)); //InOutQuad для менее линейной анимации

        if (isMerging)
        {   
            // панелька немного увеличив, меняет своё значение на новое и возращает исходный размер
            sequence.AppendCallback(() => 
            {
                image.color = ColorManager.Instance.CellColor[to.Value];
                points.text = to.Points.ToString();
                points.color = to.Value <= 2 ? ColorManager.Instance.PointsDarkColor : ColorManager.Instance.PointsLightColor;
            });

            sequence.Append(transform.DOScale(1.2f, appearTime));
            sequence.Append(transform.DOScale(1f, appearTime));
        }
        sequence.AppendCallback(() => // отображаем новое значение плитки, в которую вливаемся
        {
            to.UpdateCell();
            Destroy();
        });
    }

    // вызывается, когда на поле появляется новая рандомная плитка
    public void Appear(Cell cell)
    {
        cell.CancelAnimation(); // 
        cell.SetAnimation(this);

        image.color = ColorManager.Instance.CellColor[cell.Value];
        points.text = cell.Points.ToString();
        points.color = cell.Value <= 2 ? ColorManager.Instance.PointsDarkColor : ColorManager.Instance.PointsLightColor;

        transform.position = cell.transform.position;
        transform.localScale = Vector2.zero;

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(1.2f, appearTime * 2));
        sequence.Append(transform.DOScale(1f, appearTime * 2));
        sequence.AppendCallback(() =>
        {
            cell.UpdateCell();
            Destroy();
        });
    }

    // останавливает анимацию и уничтожает объект на сцене
    public void Destroy()
    {
        sequence.Kill(); //остановит процесс анимации
        Destroy(gameObject);
    }
}
