window.renderChart = (ctx, data) =>
{
    console.debug('Chart data sent to ChartJS: ',data);
     new Chart(ctx, data);
}

window.focusFirstInputControl = (element) => {
    const f = element.querySelector('input, select');
    if (f)
        f.focus();
}

window.getElementContent = function (element) {
    if (element) {
        return element.innerText;
    }
    return null;
};