export function addKeyUpEventListener(dotnet) {

    // todo: repeat function call while pressed
    // todo: replace depcrecated e.KeyCode
    document.addEventListener('keyup', (e) => {
        dotnet.invokeMethodAsync('OnKeyUp', e.keyCode);
    });

    window.addEventListener("keydown", function(e) {
        if(["Space","ArrowUp","ArrowDown","ArrowLeft","ArrowRight"].indexOf(e.code) > -1) {
            e.preventDefault();
        }
    }, false);
}