namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTablePaciente : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SPD_PACIENTE", "fk_id_estado_civil", "dbo.SPD_ESTADO_CIVIL");
            DropIndex("dbo.SPD_PACIENTE", "SPD_PACIENTE_SPD_ESTADO_CIVIL");
            AddColumn("dbo.SPD_PACIENTE", "estado_civil", c => c.String(maxLength: 50));
            DropColumn("dbo.SPD_PACIENTE", "fk_id_estado_civil");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SPD_PACIENTE", "fk_id_estado_civil", c => c.Int(nullable: false));
            DropColumn("dbo.SPD_PACIENTE", "estado_civil");
            CreateIndex("dbo.SPD_PACIENTE", "fk_id_estado_civil", name: "SPD_PACIENTE_SPD_ESTADO_CIVIL");
            AddForeignKey("dbo.SPD_PACIENTE", "fk_id_estado_civil", "dbo.SPD_ESTADO_CIVIL", "id_estado");
        }
    }
}
