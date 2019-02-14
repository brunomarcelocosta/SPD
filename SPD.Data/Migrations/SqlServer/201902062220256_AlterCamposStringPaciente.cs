namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCamposStringPaciente : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SPD_PACIENTE", new[] { "cpf" });
            AlterColumn("dbo.SPD_PACIENTE", "cpf", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.SPD_PACIENTE", "rg", c => c.String(maxLength: 50));
            AlterColumn("dbo.SPD_PACIENTE", "cep", c => c.String(maxLength: 50));
            CreateIndex("dbo.SPD_PACIENTE", "cpf", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.SPD_PACIENTE", new[] { "cpf" });
            AlterColumn("dbo.SPD_PACIENTE", "cep", c => c.String(maxLength: 9));
            AlterColumn("dbo.SPD_PACIENTE", "rg", c => c.String(maxLength: 10));
            AlterColumn("dbo.SPD_PACIENTE", "cpf", c => c.String(nullable: false, maxLength: 15));
            CreateIndex("dbo.SPD_PACIENTE", "cpf", unique: true);
        }
    }
}
