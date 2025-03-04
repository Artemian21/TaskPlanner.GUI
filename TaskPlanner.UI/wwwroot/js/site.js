function handleButtonClick(event) {
    // Зупиняємо поширення події, щоб картка не сприймала натискання на кнопку
    event.stopPropagation();
    alert("Button clicked!");
}
