export function addKeyPressEventListener(dotnet) {
    let timer;
    document.addEventListener('keydown', (e) => {
        if (timer) return;
        timer = setInterval(invokeDotNetKeyPressEvent, 100, dotnet, e.code);
    });

    document.addEventListener('keyup', (e) =>{
        clearInterval(timer);
        timer = null;
    });

    // Prevent browser from scroling while cerain key is pressed
    window.addEventListener("keydown", function(e) {
        if(["Space","ArrowUp","ArrowDown","ArrowLeft","ArrowRight"].indexOf(e.code) > -1) {
            e.preventDefault();
        }
    }, false);
}

function invokeDotNetKeyPressEvent(dotnet, keyCode){
    dotnet.invokeMethodAsync('OnKeyPress', keyCode);
};