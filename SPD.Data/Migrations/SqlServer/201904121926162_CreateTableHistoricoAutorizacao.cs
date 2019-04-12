namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableHistoricoAutorizacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SPD_HISTORICO_AUTORIZACAO",
                c => new
                    {
                        id_historico = c.Int(nullable: false, identity: true),
                        fk_id_paciente = c.Int(nullable: false),
                        fk_id_assinatura = c.Int(nullable: false),
                        dt_insert = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_historico, name: "SPD_HISTORICO_AUTORIZACAO_PK")
                .ForeignKey("dbo.SPD_ASSINATURA", t => t.fk_id_assinatura)
                .ForeignKey("dbo.SPD_PACIENTE", t => t.fk_id_paciente)
                .Index(t => t.fk_id_paciente, name: "SPD_HISTORICO_AUTORIZACAO_SPD_PACIENTE_FK")
                .Index(t => t.fk_id_assinatura, name: "SPD_HISTORICO_AUTORIZACAO_SPD_ASSINATURA_FK");
            
            DropColumn("dbo.SPD_PRE_CONSULTA", "autorizacao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SPD_PRE_CONSULTA", "autorizacao", c => c.Binary());
            DropForeignKey("dbo.SPD_HISTORICO_AUTORIZACAO", "fk_id_paciente", "dbo.SPD_PACIENTE");
            DropForeignKey("dbo.SPD_HISTORICO_AUTORIZACAO", "fk_id_assinatura", "dbo.SPD_ASSINATURA");
            DropIndex("dbo.SPD_HISTORICO_AUTORIZACAO", "SPD_HISTORICO_AUTORIZACAO_SPD_ASSINATURA_FK");
            DropIndex("dbo.SPD_HISTORICO_AUTORIZACAO", "SPD_HISTORICO_AUTORIZACAO_SPD_PACIENTE_FK");
            DropTable("dbo.SPD_HISTORICO_AUTORIZACAO");
        }
    }
}
