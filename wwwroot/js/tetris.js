export function draw(tetromino, position)
{
    const squares = Array.from(document.querySelectorAll('.grid div'));
    tetromino
        .map(index => squares[position + index])
        .forEach(square => square.classList.add('tetromino'));
}

export function undraw(tetromino, position){
    const squares = Array.from(document.querySelectorAll('.grid div'));
    tetromino
        .map(index => squares[position + index])
        .forEach(square => square.classList.remove('tetromino'));
}


export function checkCollision(tetromino, position, width) {
    const squares = Array.from(document.querySelectorAll('.grid div'));
    if (tetromino.some(index => squares[position + index + width].classList.contains('taken'))){
        return true;
        
        // todo: mark full block as taken by pushing an event throug Game.cs
        tetromino.forEach(index => squares[position + index].classList.add('taken'));
    }
    return false;
}




export function initialize() {

    const grid = document.querySelector('.grid');
    const score = document.querySelector('#score');
    const startButton = document.querySelector('#start-button');
    
    const squares = Array.from(document.querySelectorAll('.grid div'));

    const width = 10

    // tetromino in all four rotations
    const lTetromino = [
        [1, width+1, width*2+1, 2],
        [width, width+1, width+2, width*2+2],
        [1, width+1, width*2+1, width*2],
        [width, width*2, width*2+1, width*2+2]
    ];

    const zTetromino = [
        [0,width,width+1,width*2+1],
        [width+1, width+2,width*2,width*2+1],
        [0,width,width+1,width*2+1],
        [width+1, width+2,width*2,width*2+1]
    ];

    const tTetromino = [
        [1,width,width+1,width+2],
        [1,width+1,width+2,width*2+1],
        [width,width+1,width+2,width*2+1],
        [1,width,width+1,width*2+1]
    ];

    const oTetromino = [
        [0,1,width,width+1],
        [0,1,width,width+1],
        [0,1,width,width+1],
        [0,1,width,width+1]
    ];

    const iTetromino = [
        [1,width+1,width*2+1,width*3+1],
        [width,width+1,width+2,width+3],
        [1,width+1,width*2+1,width*3+1],
        [width,width+1,width+2,width+3]
    ];

    const tetrominoes = [lTetromino, zTetromino, tTetromino, oTetromino, iTetromino];
    
    let currentPosition = 4;
    const currentRotation = 0;
    
    // todo: randomly select a tetromino and its first rotation
    let random = Math.floor(Math.random() * tetrominoes.length);
    console.log(random);
    let currentTetromino = tetrominoes[random][currentRotation];

    // draw the tetromino
    function draw()
    {
        currentTetromino
            .map(index => squares[currentPosition + index])
            .forEach(square => square.classList.add('tetromino'));
    }
    
    function undraw()
    {
        currentTetromino
            .map(index => squares[currentPosition + index])
            .forEach(square => square.classList.remove('tetromino'));
    }
    
    draw();
    
    // make the tetromino move down every second
    const _ = setInterval(moveDown, 1000);

    // move down tetromino
    function moveDown(){
        undraw(); // current tetromino
        currentPosition += width;
        draw(); // current tetromino in new position
        freeze(); // current tetromino and draw a new tetromino (only if condition is met)
    }
    
    // freeze tetromino when reaching the bottom
    function freeze() {
        if (currentTetromino.some(index => squares[currentPosition + index + width].classList.contains('taken'))){
            currentTetromino.forEach(index => squares[currentPosition + index].classList.add('taken'));
            // start a new tetromino falling
            let random = Math.floor(Math.random() * tetrominoes.length); 
            currentTetromino = tetrominoes[random][currentRotation];
            currentPosition = 4;
            draw(); // new tetromino at the current position
        }
    }
}


