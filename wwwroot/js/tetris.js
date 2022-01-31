export function addKeyUpEventListener(dotnet) {
    document.addEventListener('keyup', (e) => {
        dotnet.invokeMethodAsync('OnKeyUp', e.key);
    });
}