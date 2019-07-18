namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Agenda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SPD_AGENDA",
                c => new
                    {
                        id_agenda = c.Int(nullable: false, identity: true),
                        fk_id_dentista = c.Int(nullable: false),
                        fk_id_paciente = c.Int(),
                        nm_paciente = c.String(maxLength: 50),
                        dt_consulta = c.DateTime(nullable: false),
                        fk_id_usuario = c.Int(nullable: false),
                        dt_insert = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_agenda, name: "SPD_AGENDA_PK")
                .ForeignKey("dbo.SPD_DENTISTA", t => t.fk_id_dentista)
                .ForeignKey("dbo.SPD_USUARIO", t => t.fk_id_usuario)
                .Index(t => t.fk_id_dentista, name: "SPD_AGENDA_SPD_DENTISTA_FK")
                .Index(t => t.fk_id_paciente, name: "SPD_AGENDA_SPD_PACIENTE_FK")
                .Index(t => t.fk_id_usuario, name: "SPD_AGENDA_SPD_USUARIO_FK");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SPD_AGENDA", "fk_id_usuario", "dbo.SPD_USUARIO");
            DropForeignKey("dbo.SPD_AGENDA", "fk_id_dentista", "dbo.SPD_DENTISTA");
            DropIndex("dbo.SPD_AGENDA", "SPD_AGENDA_SPD_USUARIO_FK");
            DropIndex("dbo.SPD_AGENDA", "SPD_AGENDA_SPD_PACIENTE_FK");
            DropIndex("dbo.SPD_AGENDA", "SPD_AGENDA_SPD_DENTISTA_FK");
            DropTable("dbo.SPD_AGENDA");
        }
    }
}
