export function initDragDrop(element, objRef) {
    console.log('initDragDrop');
    const dragStartListener = event => {
        const sbiId = event.target.getAttribute('data-sbi-id');

        // prevent dragging child elements
        if (!sbiId) {
            event.preventDefault();
            return;
        }

        event.dataTransfer.setData("text/plain", sbiId);
        event.dataTransfer.dropEffect = "move";
    };

    const dragOverListener = event => {
        event.preventDefault();
        event.dataTransfer.dropEffect = "move";
    };

    const dragEnterListener = event => {
        event.preventDefault();
        event.target.classList.add('dragenter');
    };

    const dragLeaveListener = event => {
        event.preventDefault();
        event.dataTransfer.dropEffect = "move";
        event.target.classList.remove('dragenter');
    };

    const dropListener = event => {
        event.preventDefault();
        event.target.classList.remove('dragenter');

        const pbiData = event.target.getAttribute('data-pbi');

        // handle case where drop on some invalid target
        if (!pbiData)
            return;

        const data = pbiData.split(',');
        const sbiId = event.dataTransfer.getData("text/plain");

        objRef.invokeMethodAsync("DropJS", data[0], Number(data[1]), sbiId);
    };

    // drag sources
    const sbis = document.querySelectorAll('[data-sbi-id]');
    sbis.forEach(item => item.addEventListener('dragstart', dragStartListener));

    // drop targets
    const pbis = document.querySelectorAll('[data-pbi]');
    pbis.forEach(item => {
        item.addEventListener('dragover', dragOverListener);
        item.addEventListener('drop', dropListener);
        item.addEventListener('dragenter', dragEnterListener);
        item.addEventListener('dragleave', dragLeaveListener);
    });

    return {
        stop: () => {
            pbis.forEach(item => {
                item.removeEventListener('dragleave', dragLeaveListener);
                item.removeEventListener('dragenter', dragEnterListener);
                item.removeEventListener('drop', dropListener);
                item.removeEventListener('dragover', dragOverListener);
            });
            sbis.forEach(item => item.removeEventListener('dragstart', dragStartListener));
        }
    };
}