const { Client } = require('pg');
const express = require('express');
const app = express();
app.use(express.json());

const con = new Client({
    host: "localhost",
    user: "postgres",
    port: 5432,
    password: "admin123",
    database: "stockflow"
});

con.connect()
    .then(() => console.log("Connected to PostgreSQL"))
    .catch(err => console.error('Connection error', err.stack));

// 🟢 Criar item
app.post('/items', (request, response) => {
    console.log("Recebido no CREATE")
    console.log(request.body);

    const Id = request.body.Id || request.body.id;
    const Name = request.body.Name || request.body.name;
    const Amount = request.body.Amount || request.body.amount;
    const MinAmount = request.body.MinAmount || request.body.minAmount;
    const Category = request.body.Category || request.body.category;
    const Supplier = request.body.Supplier || request.body.supplier;

    if (!Id || !Name || !Amount || !MinAmount || !Category || !Supplier) {
        return response.status(400).json({ error: "Todos os campos são obrigatórios!" });
    }

    const insert_query = 'INSERT INTO item (Id, Name, Amount, MinAmount, Category, Supplier) VALUES ($1, $2, $3, $4, $5, $6)';

    con.query(insert_query, [Id, Name, Amount, MinAmount, Category, Supplier], (error, result) => {
        if (error) {
            console.error("Erro de consulta:", error);
            return response.status(500).json({ error: "Erro ao criar item", details: error.message });
        }
        response.status(201).json({ message: "Item criado com sucesso" });
    });
});


// 🟢 Buscar todos os itens
app.get('/items', (request, response) => {
    const get_query = 'SELECT * FROM item';

    con.query(get_query, (error, result) => {
        if (error) {
            console.error("Erro de consulta:", error);
            return response.status(500).json({ error: "Erro ao buscar itens", details: error.message });
        }
        response.status(200).json({ message: "Itens encontrados com sucesso", items: result.rows });
    });
});

// 🟢 Buscar item por ID
app.get('/items/:id', (request, response) => {
    const { id } = request.params;
    const query = 'SELECT * FROM item WHERE Id = $1';

    con.query(query, [id], (error, result) => {
        if (error) {
            return response.status(500).json({ error: "Erro ao buscar item", details: error.message });
        }

        if (result.rows.length === 0) {
            return response.status(404).json({ message: "Item não encontrado" });
        }

        response.status(200).json(result.rows[0]);
    });
});

// 🟢 Atualizar item por ID
app.put('/items/:id', (request, response) => {
    console.log("Recebido no UPDATE:");
    console.log(request.body);

    const { id } = request.params; 
    const { Name, Amount, MinAmount, Category, Supplier } = request.body;

    const update_query = `
        UPDATE item
        SET Name = $1, Amount = $2, MinAmount = $3, Category = $4, Supplier = $5
        WHERE Id = $6
    `;

    const values = [Name, Amount, MinAmount, Category, Supplier, id];

    con.query(update_query, values, (error, result) => {
        if (error) {
            return response.status(500).json({ error: "Erro ao atualizar item", details: error.message });
        }

        if (result.rowCount === 0) {
            return response.status(404).json({ message: "Item não encontrado" });
        }

        response.status(200).json({ message: "Item atualizado com sucesso" });
    });
});

// 🟢 Deletar item por ID
app.delete('/items/:id', (request, response) => {
    const { id } = request.params;

    const delete_query = 'DELETE FROM item WHERE Id = $1';

    con.query(delete_query, [id], (error, result) => {
        if (error) {
            return response.status(500).json({ error: "Erro ao deletar item", details: error.message });
        }

        if (result.rowCount === 0) {
            return response.status(404).json({ message: "Item não encontrado" });
        }

        response.status(200).json({ message: "Item deletado com sucesso" });
    });
});

app.listen(3000, () => {
    console.log("Server Running on port 3000");
});
