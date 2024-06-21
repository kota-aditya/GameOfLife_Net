# Conway's Game of Life API

This project is an implementation of Conway's Game of Life as a RESTful API using ASP.NET Core. The API allows users to upload board states, retrieve the next state, retrieve a specific state after a number of generations, and get the final state of a board.

## Features

- **Upload Board State:** Upload a new board state.
- **Get Next State:** Retrieve the next state of the board.
- **Get Nth State:** Retrieve the state of the board after a specified number of generations.
- **Get Final State:** Retrieve the final state of the board after a specified number of generations.

## Prerequisites

- [.NET 7.0 or 8.0 SDK](https://dotnet.microsoft.com/download/dotnet)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (Community, Professional, or Enterprise)
- [Postman](https://www.postman.com/downloads/) or any other API testing tool (optional but recommended)

## Getting Started

### Setting Up the Project

1. **Clone the repository:**

    ```bash
    git clone https://github.com/your-username/game-of-life-api.git
    cd game-of-life-api
    ```

2. **Open the project in Visual Studio:**

    Open `GameOfLife.sln` in Visual Studio 2022.

3. **Restore the dependencies:**

    Visual Studio should automatically restore the necessary NuGet packages. If not, go to `Tools > NuGet Package Manager > Package Manager Console` and run:

    ```bash
    dotnet restore
    ```

### Running the Application

1. **Run the application:**

    Press `F5` or click the "Start Debugging" button in Visual Studio to run the application.

2. **API Endpoints:**

    The application will start and should be accessible at `https://localhost:5001` or `http://localhost:5000`.

## API Documentation

### Endpoints

#### Upload a New Board State

- **URL:** `/api/GameOfLife/upload`
- **Method:** `POST`
- **Request Body:**

    ```json
    {
      "State": [
        [0, 1, 0],
        [0, 1, 0],
        [0, 1, 0]
      ],
      "Rows": 3,
      "Columns": 3
    }
    ```

- **Response:**

    ```json
    {
      "Id": "generated-board-id"
    }
    ```

#### Get the Next State

- **URL:** `/api/GameOfLife/{boardId}/next`
- **Method:** `GET`
- **Response:**

    ```json
    {
      "Id": "board-id",
      "State": [
        [0, 0, 0],
        [1, 1, 1],
        [0, 0, 0]
      ],
      "Rows": 3,
      "Columns": 3
    }
    ```

#### Get the Nth State

- **URL:** `/api/GameOfLife/{boardId}/states/{n}`
- **Method:** `GET`
- **Response:**

    ```json
    {
      "Id": "board-id",
      "State": [
        [0, 1, 0],
        [0, 1, 0],
        [0, 1, 0]
      ],
      "Rows": 3,
      "Columns": 3
    }
    ```

#### Get the Final State

- **URL:** `/api/GameOfLife/{boardId}/final/{maxSteps}`
- **Method:** `GET`
- **Response:**

    ```json
    {
      "Id": "board-id",
      "State": [
        [0, 0, 0],
        [1, 1, 1],
        [0, 0, 0]
      ],
      "Rows": 3,
      "Columns": 3
    }
    ```

### HTTP File for Testing

You can use the following HTTP file (`gameoflife.http`) to test the API endpoints in Visual Studio Code with the REST Client extension or any other HTTP client:

```http
### Upload a new board state
POST https://localhost:5001/api/GameOfLife/upload
Content-Type: application/json

{
  "State": [
    [0, 1, 0],
    [0, 1, 0],
    [0, 1, 0]
  ],
  "Rows": 3,
  "Columns": 3
}

###

### Get the next state for a board
GET http://localhost:5000/api/GameOfLife/{{boardId}}/next

###

### Get the nth state for a board
GET http://localhost:5000/api/GameOfLife/{{boardId}}/states/5

###

### Get the final state for a board
GET http://localhost:5000/api/GameOfLife/{{boardId}}/final/10


## Additional Notes
I see a strong reason to implement concurrency. It can acheived in different ways which I can explain when we connect.