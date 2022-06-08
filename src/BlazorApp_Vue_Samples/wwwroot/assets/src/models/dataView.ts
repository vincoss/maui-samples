export class DataContext
{
    public definition: Definition = new Definition();
    public data: Data = new Data();

    public save(data: Data) { return; }
    public validate(data: Data) { return; }
}

export class Definition
{
    public value: string | null = null;
}

export class Data
{
    public value: string | null = null;
}