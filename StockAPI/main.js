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
    console.log("Recebido no CREATE", request.body);

    const Id = request.body.Id || request.body.id;
    const Name = request.body.Name || request.body.name;
    const Amount = request.body.Amount || request.body.amount;
    const MinAmount = request.body.MinAmount || request.body.minAmount;
    const Category = request.body.Category || request.body.category;
    const Supplier = request.body.Supplier || request.body.supplier;
    let Price = request.body.Price || request.body.price;

    if (!Id || !Name || !Amount || !MinAmount || !Category || !Supplier || !Price) {
        return response.status(400).json({ error: "Todos os campos são obrigatórios!" });
    }

    Price = parseFloat(Price);

    if (isNaN(Price)) {
        return response.status(400).json({ error: "Preço inválido" });
    }

    console.log("Valor do Preço:", Price);

    const insert_query = 'INSERT INTO item (Id, Name, Amount, MinAmount, Category, Supplier, Price) VALUES ($1, $2, $3, $4, $5, $6, $7)';

    con.query(insert_query, [Id, Name, Amount, MinAmount, Category, Supplier, Price], (error, result) => {
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
    console.log("Recebido no UPDATE:", request.body);

    const { id } = request.params; 
    const { name, amount, minAmount, category, supplier, price } = request.body;

    // Convertendo o preço para número
    const priceValue = parseFloat(price);
    if (isNaN(priceValue)) {
        return response.status(400).json({ error: "Preço inválido" });
    }

    // Convertendo o ID para UUID
    const uuid = id.toString();

    const update_query = `
    UPDATE item
    SET name = $1, amount = $2, minamount = $3, category = $4, supplier = $5, price = $6
    WHERE id = $7::UUID
    `;

    const values = [name, amount, minAmount, category, supplier, priceValue, uuid];

    // Executando a query
    con.query(update_query, values, (error, result) => {
        if (error) {
            console.error("Erro ao atualizar item:", error);
            return response.status(500).json({ error: "Erro ao atualizar item", details: error.message });
        }

        console.log("Linhas afetadas:", result.rowCount);

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
